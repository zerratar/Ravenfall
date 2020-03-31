using System;

namespace Shinobytes.Ravenfall.Core
{
    public static class GameMath
    {
        public const int MaxLevel = 170;

        private static decimal[] ExperienceArray = new decimal[MaxLevel];

        static GameMath()
        {
            var l = 0L;
            for (var i1 = 0; i1 < MaxLevel; i1++)
            {
                var j1 = i1 + 1M;
                var l1 = (long)(j1 + (decimal)(300D * Math.Pow(2D, (double)(j1 / 7M))));
                l += l1;
                ExperienceArray[i1] = (decimal)((l & 0xffffffffc) / 4d);
            }
        }

        public static int MaxHit(
            int strength, int weaponPower,
            bool burst, bool superhuman, bool ultimate, int bonus)
        {
            var prayer = AddPrayers(burst, superhuman, ultimate);
            var newStrength = strength * prayer + bonus;

            var w1 = weaponPower * 0.00175D;
            var w2 = w1 + 0.1d;
            var w3 = newStrength * w2 + 1.05D;
            return (int)(w3 * 0.95d);
        }

        public static double AddPrayers(bool first, bool second, bool third)
        {
            if (third) return 1.15d;
            if (second) return 1.1d;
            if (first) return 1.05d;
            return 1.0d;
        }

        public static int CombatExperience(int combatLevel)
        {
            return (int)((combatLevel * 10 + 10) * 1.5D);
        }

        public static int ExperienceToLevel(decimal exp)
        {
            for (int level = 0; level < MaxLevel - 1; level++)
            {
                if (exp >= ExperienceArray[level])
                    continue;
                return (level + 1);
            }
            return MaxLevel;
        }

        public static decimal LevelToExperience(int level)
        {
            return level - 2 < 0 ? 0 : ExperienceArray[level - 2];
        }

        public static decimal GetFishingExperience(int level)
        {
            if (level < 15) return 25;
            if (level < 30) return 37.5m;
            if (level < 45) return 100;
            if (level < 60) return 175;
            if (level < 75) return 250;

            return 10;
        }
        public static decimal GetFarmingExperience(int level)
        {
            if (level < 15) return 25;
            if (level < 30) return 37.5m;
            if (level < 45) return 100;
            if (level < 60) return 175;
            if (level < 75) return 250;

            return 10;
        }
        public static decimal GetWoodcuttingExperience(int level)
        {
            /*
                Item   | Lv Req | Exp | Fatigue
                Logs	| 1	     | 25  | 0.533%
                Oak logs	15	37.5	0.8%
                Willow logs	30	62.5	1.333%
                Maple logs	45	100	2.133%
                Yew logs	60	175	3.733%
                Magic logs	75	250	5.333%
            */

            if (level < 15) return 25;
            if (level < 30) return 37.5m;
            if (level < 45) return 100;
            if (level < 60) return 175;
            if (level < 75) return 250;

            return 25;
        }
    }
}
