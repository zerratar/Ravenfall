using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Shinobytes.Ravenfall.RavenNet.Serializers
{
    public class BinarySerializer : IBinarySerializer
    {
        private readonly ConcurrentDictionary<string, MethodInfo> writeMethodCache = new ConcurrentDictionary<string, MethodInfo>();
        private readonly ConcurrentDictionary<string, PropertyInfo[]> writePropsCache = new ConcurrentDictionary<string, PropertyInfo[]>();
        private readonly ConcurrentDictionary<string, FieldInfo[]> writeFieldsCache = new ConcurrentDictionary<string, FieldInfo[]>();

        private readonly ConcurrentDictionary<string, MethodInfo> readMethodCache = new ConcurrentDictionary<string, MethodInfo>();
        private readonly ConcurrentDictionary<string, PropertyInfo[]> readPropsCache = new ConcurrentDictionary<string, PropertyInfo[]>();
        private readonly ConcurrentDictionary<string, FieldInfo[]> readFieldsCache = new ConcurrentDictionary<string, FieldInfo[]>();

        public object Deserialize(byte[] data, Type type)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                var res = Deserialize(br, type);
                if (res != null) return res;
                return DeserializeComplex(br, type);
            }
        }

        public byte[] Serialize(object data)
        {
            if (data == null) return new byte[0];
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                Serialize(bw, data);
                return ms.ToArray();
            }
        }

        #region Serialization

        private void Serialize(BinaryWriter bw, object data)
        {
            var type = data.GetType();

            if (SerializeSpecial(bw, data, type)) return;

            if (!writeMethodCache.TryGetValue(type.FullName, out var targetMethod))
            {
                targetMethod = bw.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(x => MatchWriteMethod(x, type));
                writeMethodCache[type.FullName] = targetMethod;
            }

            if (targetMethod != null)
            {
                targetMethod.Invoke(bw, new object[] { data });
                return;
            }


            SerializeComplex(bw, data, data.GetType());
        }

        private void Serialize(BinaryWriter bw, object data, PropertyInfo property)
        {
            var type = property.PropertyType;
            var value = property.GetValue(data);
            Serialize(bw, value, type);
        }

        private void Serialize(BinaryWriter bw, object data, FieldInfo field)
        {
            var type = field.FieldType;
            var value = field.GetValue(data);
            Serialize(bw, value, type);
        }

        private void Serialize(BinaryWriter bw, object value, Type type)
        {
            if (SerializeSpecial(bw, value, type))
                return;

            if (!writeMethodCache.TryGetValue(type.FullName, out var targetMethod))
            {
                targetMethod = bw.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(x => MatchWriteMethod(x, type));
                writeMethodCache[type.FullName] = targetMethod;
            }

            if (targetMethod != null)
            {
                targetMethod.Invoke(bw, new[] { value });
                return;
            }

            SerializeComplex(bw, value, type);
        }

        private void SerializeComplex(BinaryWriter bw, object data, Type type)
        {
            if (!type.IsValueType)
            {
                var hasData = data != null ? 1 : 0;
                bw.Write((byte)hasData);
            }

            if (!writePropsCache.TryGetValue(type.Name, out var props))
            {
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                writePropsCache[type.Name] = props;
            }

            //var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.CanWrite && prop.CanRead)
                {
                    Serialize(bw, data, prop);
                }
            }

            if (!writeFieldsCache.TryGetValue(type.Name, out var fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                writeFieldsCache[type.Name] = fields;
            }
            //var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    Serialize(bw, data, field);
                }
            }
        }

        private bool SerializeSpecial(BinaryWriter bw, object value, Type type)
        {
            if (type.IsArray)
            {
                if (type.HasElementType)
                {
                    var elementType = type.GetElementType();
                    if (SerializeArray(bw, value, elementType, type))
                        return true;
                }
            }

            if (type.IsGenericType)
            {
                var typeArgs = type.GenericTypeArguments;
                var baseType = type.GetGenericTypeDefinition();
                if (SerializeGeneric(bw, value, baseType, typeArgs))
                    return true;
            }

            if (type == typeof(string))
            {
                var hasValue = value != null;
                bw.Write((byte)(hasValue ? 1 : -1));
                if (hasValue)
                    bw.Write(value.ToString());
                return true;
            }

            if (type == typeof(Guid))
            {
                bw.Write(((Guid)value).ToByteArray());
                return true;
            }

            if (type == typeof(DateTime))
            {
                bw.Write(((DateTime)value).ToBinary());
                return true;
            }

            if (type == typeof(TimeSpan))
            {
                bw.Write(((TimeSpan)value).Ticks);
                return true;
            }

            return false;
        }

        private bool SerializeGeneric(BinaryWriter bw, object value, Type baseType, Type[] typeArgs)
        {
            // check if its a collection type, so we can iterate it
            if (value == null)
            {
                bw.Write(-1);
                return true;
            }

            if (value is IDictionary dictionary)
            {
                bw.Write(dictionary.Count);
                foreach (var key in dictionary.Keys)
                {
                    Serialize(bw, key);
                    Serialize(bw, dictionary[key]);
                }
                return true;
            }

            if (value is IEnumerable enumerable)
            {
                var items = enumerable.Cast<object>().ToList();
                bw.Write(items.Count);
                foreach (var item in items)
                {
                    Serialize(bw, item);
                }

                return true;
            }

            return false;
        }
        private bool SerializeArray(BinaryWriter bw, object value, Type elementType, Type type)
        {
            if (elementType == null || !type.IsArray) return false;
            if (value == null)
            {
                bw.Write(-1);
                return true;
            }

            var array = (Array)value;
            var len = array.Length;
            bw.Write(len);
            for (var i = 0; i < len; ++i)
            {
                Serialize(bw, array.GetValue(i));
            }
            return true;
        }

        #endregion

        #region Deserialization

        private object Deserialize(BinaryReader br, PropertyInfo property)
        {
            return Deserialize(br, property.PropertyType);
        }

        private object Deserialize(BinaryReader br, FieldInfo field)
        {
            var type = field.FieldType;
            return Deserialize(br, type);
        }

        private object Deserialize(BinaryReader br, Type type)
        {
            if (TryDeserializeSpecial(br, type, out var res))
            {
                return res;
            }

            if (!readMethodCache.TryGetValue(type.Name, out var targetMethod))
            {
                targetMethod = br.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(x => MatchReadName(x.Name, type.Name));
                readMethodCache[type.Name] = targetMethod;
            }

            if (targetMethod != null)
            {
                return targetMethod.Invoke(br, null);
            }

            return DeserializeComplex(br, type);
        }

        private object DeserializeComplex(BinaryReader br, Type type)
        {
            // checks if the reference type is null or not
            if (!type.IsValueType)
            {
                if (br.ReadByte() == 0)
                {
                    return null;
                }
            }

            var obj = FormatterServices.GetUninitializedObject(type);

            if (!readPropsCache.TryGetValue(type.Name, out var props))
            {
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                readPropsCache[type.Name] = props;
            }

            //var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = Deserialize(br, prop);
                    prop.SetValue(obj, value);
                }
            }

            if (!readFieldsCache.TryGetValue(type.Name, out var fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                readFieldsCache[type.Name] = fields;
            }

            //var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                // still need to deserialize so we read from the stream
                var value = Deserialize(br, field);
                if (!field.IsInitOnly)
                {
                    field.SetValue(obj, value);
                }
            }

            return obj;
        }

        private bool TryDeserializeArray(BinaryReader br, Type elementType, Type arrayType, out object result)
        {
            result = null;
            if (elementType == null || !arrayType.IsArray) return false;

            var size = br.ReadInt32();
            if (size == -1) return true;
            var array = (Array)Activator.CreateInstance(arrayType, new object[] { size });
            result = array;

            if (size == 0) return true;

            for (var i = 0; i < size; ++i)
            {
                var value = Deserialize(br, elementType);
                array.SetValue(value, i);
            }

            return true;
        }

        private bool TryDeserializeGeneric(BinaryReader br, Type baseType, Type[] typeArgs, out object result)
        {
            result = null;
            var size = br.ReadInt32();
            if (size <= 0)
            {
                return true;
            }

            if (typeof(IDictionary).IsAssignableFrom(baseType))
            {
                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeArgs);
                var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);
                for (var i = 0; i < size; ++i)
                {
                    var key = Deserialize(br, typeArgs[0]);
                    var value = Deserialize(br, typeArgs[1]);
                    dictionary[key] = value;
                }

                result = dictionary;
                return true;
            }

            if (typeof(IList).IsAssignableFrom(baseType) || typeof(IReadOnlyList<>).IsAssignableFrom(baseType) || typeof(IList<>).IsAssignableFrom(baseType) || typeof(ICollection<>).IsAssignableFrom(baseType))
            {
                var listType = typeof(List<>).MakeGenericType(typeArgs);
                var list = (IList)Activator.CreateInstance(listType);
                for (var i = 0; i < size; ++i)
                {
                    list.Add(Deserialize(br, typeArgs[0]));
                }

                result = list;
                return true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(baseType))
            {
                throw new NotImplementedException("Direct IEnumerable's are not supported yet.");
            }

            return false;
        }

        private bool TryDeserializeSpecial(BinaryReader br, Type type, out object result)
        {
            result = null;

            if (type.IsArray)
            {
                if (type.HasElementType)
                {
                    var elementType = type.GetElementType();
                    if (TryDeserializeArray(br, elementType, type, out result))
                        return true;
                }
            }

            if (type.IsGenericType)
            {
                var typeArgs = type.GenericTypeArguments;
                var baseType = type.GetGenericTypeDefinition();
                if (TryDeserializeGeneric(br, baseType, typeArgs, out result))
                    return true;
            }

            if (type == typeof(Guid))
            {
                result = new Guid(br.ReadBytes(16));
                return true;
            }

            if (type == typeof(string))
            {
                var hasValue = br.ReadByte() == 1;
                if (hasValue) result = br.ReadString();
                return true;
            }

            if (type == typeof(DateTime))
            {
                result = DateTime.FromBinary(br.ReadInt64());
                return true;
            }

            if (type == typeof(TimeSpan))
            {
                result = TimeSpan.FromTicks(br.ReadInt64());
                return true;
            }
            return false;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static bool MatchReadName(string methodName, string typeName)
        private static bool MatchReadName(ReadOnlySpan<char> methodName, ReadOnlySpan<char> typeName)
        {
            if (typeName.Equals("float", StringComparison.OrdinalIgnoreCase) && methodName.Equals("ReadSingle")) return true;
            if (!methodName.StartsWith("Read", StringComparison.OrdinalIgnoreCase)) return false;
            return methodName.EndsWith(typeName, StringComparison.OrdinalIgnoreCase);
            //return methodName.StartsWith("Read" + typeName, StringComparison.OrdinalIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool MatchWriteMethod(MethodInfo x, Type type)
        {
            if (x.Name != "Write") return false;
            var parameters = x.GetParameters();
            if (parameters.Length != 1) return false;
            return parameters.All(y => y.ParameterType == type);
        }
    }
}