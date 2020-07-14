using System;
using System.Linq;

namespace GameServer
{
    public static class Utility
    {
        private static readonly Random random = new Random();

        public static T Random<T>(params T[] items)
        {
            return items
                .OrderBy(x => random.NextDouble()).First();
        }

        public static T Random<T>()
            where T : struct, IConvertible
        {
            return Enum
                .GetValues(typeof(T)).Cast<T>()
                .OrderBy(x => random.NextDouble()).First();
        }

        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
