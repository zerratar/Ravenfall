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

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return
                System.Math.Abs(a.X - b.X) <= 0.00001 &&
                System.Math.Abs(a.Y - b.Y) <= 0.00001 &&
                System.Math.Abs(a.Z - b.Z) <= 0.00001;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Magnitude;
        }

        public static implicit operator UnityEngine.Vector3(Vector3 obj)
        {
            return new UnityEngine.Vector3
            {
                x = obj.X,
                y = obj.Y,
                z = obj.Z
            };
        }

        public static implicit operator Vector3(UnityEngine.Vector3 obj)
        {
            return new Vector3
            {
                X = obj.x,
                Y = obj.y,
                Z = obj.z
            };
        }

        public override string ToString()
        {
            return $"{{ X: {X}, Y: {Y}, Z: {Z} }}";
        }
    }
}
