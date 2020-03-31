using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.Data
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly ConcurrentDictionary<Type, PropertyInfo[]> propertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

        private readonly Type[] numericTypes = new Type[] {
            typeof(byte), typeof(sbyte), typeof(ushort), typeof(short), typeof(uint), typeof(int), typeof(ulong), typeof(long), typeof(decimal), typeof(float), typeof(double),
            typeof(byte?), typeof(sbyte?), typeof(ushort?), typeof(short?), typeof(uint?), typeof(int?), typeof(ulong?), typeof(long?), typeof(decimal?), typeof(float?), typeof(double?)
        };

        public SqlSaveQuery Build(EntityStoreItems saveData)
        {
            switch (saveData.State)
            {
                case EntityState.Added: return BuildInsertQuery(saveData);
                case EntityState.Deleted: return BuildDeleteQuery(saveData);
                case EntityState.Modified: return BuildUpdateQuery(saveData);
                default: return null;
            }
        }

        private SqlSaveQuery BuildUpdateQuery(EntityStoreItems saveData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN TRANSACTION;");
            foreach (var entity in saveData.Entities)
            {
                var type = entity.GetType();
                var properties = GetProperties(type);
                var idProperty = GetProperty(type, "Id");
                var propertySets = GetSqlReadyPropertySet(entity, properties.Where(x => x.Name != "Id").ToArray());
                sb.Append($"UPDATE [{type.Name}] ");
                sb.Append($"SET {string.Join(",", propertySets)} ");
                sb.AppendLine($"WHERE Id = {GetSqlReadyPropertyValue(idProperty.PropertyType, idProperty.GetValue(entity))};");
            }

            sb.AppendLine("COMMIT;");
            return new SqlSaveQuery(sb.ToString());
        }

        private SqlSaveQuery BuildDeleteQuery(EntityStoreItems saveData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN TRANSACTION;");
            foreach (var group in saveData.Entities.GroupBy(x => x.GetType()))
            {
                var type = group.Key;
                var idProperty = GetProperty(type, "Id");
                //var properties = GetProperties(type);            
                sb.AppendLine($"DELETE FROM [{group.Key.Name}] WHERE ");

                var wheres = group.Select(entity => "(Id = " + GetSqlReadyPropertyValue(idProperty.PropertyType, idProperty.GetValue(entity)) + ")").ToList();
                sb.Append(string.Join(" OR \r\n", wheres));

                sb.AppendLine(";");
            }
            sb.AppendLine("COMMIT;");
            return new SqlSaveQuery(sb.ToString());
        }

        private SqlSaveQuery BuildInsertQuery(EntityStoreItems saveData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN TRANSACTION;");
            foreach (var group in saveData.Entities.GroupBy(x => x.GetType()))
            {
                var type = group.Key;
                var properties = GetProperties(type);
                var propertyNames = string.Join(", ", properties.Select(x => x.Name));
                sb.AppendLine($"INSERT INTO [{group.Key.Name}] ({propertyNames}) VALUES ");

                var rows = group.Select(x => "(" + string.Join(",", GetSqlReadyPropertyValues(x, properties)) + ")");
                var line = string.Join(",\r\n", rows);
                sb.Append(line);
                sb.AppendLine(";");
            }
            sb.AppendLine("COMMIT;");
            return new SqlSaveQuery(sb.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetSqlReadyPropertyValues(IEntity entity, PropertyInfo[] properties)
        {
            return properties.Select(x => GetSqlReadyPropertyValue(x.PropertyType, x.GetValue(entity)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetSqlReadyPropertySet(IEntity entity, PropertyInfo[] properties)
        {
            return properties.Select(x => x.Name + " = " + GetSqlReadyPropertyValue(x.PropertyType, x.GetValue(entity)));
        }

        private string GetSqlReadyPropertyValue(Type type, object value)
        {
            if (value == null) return "NULL";
            if (type == typeof(string) || type == typeof(char)
                || type == typeof(DateTime) || type == typeof(TimeSpan)
                || type == typeof(DateTime?) || type == typeof(TimeSpan?)
                || type == typeof(Guid?) || type == typeof(Guid))
            {

                return $"'{Sanitize(value?.ToString())}'";
            }

            if (type.IsEnum)
            {
                return ((int)value).ToString();
            }

            if (numericTypes.Any(x => x == type))
            {
                return value.ToString().Replace(',', '.');
            }

            if (typeof(bool) == type)
            {
                return (bool)value == true ? "1" : "0";
            }

            if (typeof(bool?) == type)
            {
                var b = (value as bool?);
                return b.GetValueOrDefault() ? "1" : "0";
            }

            return "NULL";
        }

        private string Sanitize(string value)
        {
            // TODO: Implement
            //       We should be using sqlparameters but how do we bulk that properly?
            return value?.Replace("'", "''");
        }

        private Dictionary<string, object> GetKeyValueMap(IEntity entity, PropertyInfo[] properties)
        {
            var dict = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                dict[property.Name] = property.GetValue(entity);
            }
            return dict;
        }

        private PropertyInfo[] GetProperties(Type type)
        {
            if (propertyCache.TryGetValue(type, out var properties))
            {
                return properties;
            }

            return propertyCache[type] = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        private PropertyInfo GetProperty(Type type, string propertyName)
        {
            return GetProperties(type).FirstOrDefault(x => x.Name == propertyName);
        }
    }
}