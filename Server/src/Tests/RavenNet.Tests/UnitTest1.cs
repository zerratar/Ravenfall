using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;

namespace RavenNet.Tests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void TestBadSerialization()
        {
            var payload = new byte[] { 1, 3, 0, 0, 0, 1, 10, 112, 108, 97, 121, 101, 114, 51, 55, 54, 49, 0, 35, 208, 146, 63, 87, 22, 133, 63, 48, 213, 164, 63, };

            var targetType = typeof(PlayerAdd);
            var serializer = new BinarySerializer();
            var data = serializer.Deserialize(payload, targetType);



            //var payload = new byte[] { 1, 10, 0, 0, 0, 1, 10, 112, 108, 97, 121, 101, 114, 55, 50, 52, 54, 0, 123, 136, 20, 64, 87, 22, 133, 63, 75, 87, 173, 64, 64, 126, 191, 64, 162, 61, 15, 66, 1, 77, 206, 88, 64, 233, 121, 246, 164, 50, 12, 154, 64, 199, 93, 188, 64, 222, 153, 10, 66, };

            //var targetType = typeof(PlayerAdd);
            //var serializer = new BinarySerializer();
            //var data = serializer.Deserialize(payload, targetType);

        }

        [TestMethod]
        public void SerializeDeserializeFullPacket()
        {
            var serializer = new BinarySerializer();
            var lookup = new NetworkPacketTypeRegistry();
            lookup.Register<Test>(1);

            var packetSerializer = new NetworkPacketSerializer(null, lookup, serializer);

            var packet = new NetworkPacket();
            packet.Id = 1;
            packet.Data = new Test();
            var data = packetSerializer.Serialize(packet);
            var result = packetSerializer.Deserialize(data);
            Assert.AreEqual(typeof(Test).FullName, result.Data.GetType().FullName);
            Assert.AreEqual("Hello, world!", ((Test)result.Data).Value);
        }

    }

    public class Test
    {
        public string Value { get; set; } = "Hello, world!";
    }


    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        public float SqrtMagnitude => X * X + Y * Y + Z * Z;

        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X - b.X,
            Y = a.Y - b.Y,
            Z = a.Z - b.Z
        };

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X + b.X,
            Y = a.Y + b.Y,
            Z = a.Z + b.Z
        };

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Magnitude;
        }

        public override string ToString()
        {
            return $"{{x: {X}, y: {Y}, z: {Z}}}";
        }
    }
    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
    }

}
