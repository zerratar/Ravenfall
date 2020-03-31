using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
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
}