using System;

namespace Shinobytes.Ravenfall.Core
{
    public static class GameMath
    {
        public const int MaxLevel = 170;

        private static decimal[] ExperienceArray = new decimal[MaxLevel];
        private static readonly Random random = new Random();

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

        public static float CalculateMagicDamage(int aMagic, int bArmorPower)
        {
            return CalculateCastDamage(bArmorPower, aMagic, MagicPower(aMagic / 10));
        }

        public static float CalculateRangedDamage(int aRanged, int bArmorPower)
        {
            return CalculateCastDamage(bArmorPower, aRanged, ArrowPower(aRanged / 10));
        }

        public static float CalculateDamage(
            int aAttack,
            int aStrength,
            int aCombatStyle,
            int aWeaponPower,
            int aWeaponAim,
            int bStrength,
            int bDefense,
            int bCombatStyle,
            int bArmorPower,
            bool attackerIsNpc = false,
            bool defenderIsNpc = false)
        {
            var burst = false;
            var superhuman = false;
            var ultimate = false;
            var bonus = StyleBonus(aCombatStyle, 2);
            var max = MaxHit(aStrength, aWeaponPower, burst, superhuman, ultimate, bonus);

            var attackPrayers = AddPrayers(burst, superhuman, ultimate);

            var newAtt = (int)(attackPrayers * (aAttack / 0.8D)
                         + (random.Next(0, 4) == 0
                             ? aWeaponPower
                             : aWeaponAim / 2.5d)
                         + (aCombatStyle == 1 && random.Next(0, 2) == 0 ? 4 : 0)
                         + (random.Next(0, 100) <= 10 ? (aStrength / 5D) : 0)
                         + (StyleBonus(aCombatStyle, 0) * 2));

            var defensePrayers = AddPrayers(burst, superhuman, ultimate);

            var newDef = (int)(defensePrayers
                         * ((random.Next(0, 100) <= 5 ? 0 : bDefense) * 1.1D)
                         + ((random.Next(0, 100) <= 5 ? 0 : bArmorPower) / 2.75D)
                         + (bStrength / 4D) + (StyleBonus(bCombatStyle, 1) * 2));

            var hitChance = random.Next(0, 100) + (newAtt - newDef);

            if (attackerIsNpc)
            {
                hitChance -= 5;
            }

            if (random.Next(0, 100) <= 10)
            {
                hitChance += 20;
            }

            if (hitChance > (defenderIsNpc ? 40 : 50))
            {
                var newMax = 0;
                var maxProb = 5;
                var nearMaxProp = 7;
                var avProb = 73;
                var lowHit = 10;

                var shiftValue = (int)Math.Round(bArmorPower * 0.02d, MidpointRounding.AwayFromZero);

                maxProb -= shiftValue;
                nearMaxProp -= (int)Math.Round(shiftValue * 1.5, MidpointRounding.AwayFromZero);
                avProb -= (int)Math.Round(shiftValue * 2.0, MidpointRounding.AwayFromZero);
                lowHit += (int)Math.Round(shiftValue * 3.5, MidpointRounding.AwayFromZero);

                var hitRange = random.Next(0, 100);
                if (hitRange >= (100 - maxProb))
                {
                    return max;
                }

                if (hitRange >= (100 - nearMaxProp))
                {

                    return (int)Math.Round(Math.Abs(max - max * (random.Next(0, 10) * 0.01D)), MidpointRounding.AwayFromZero);
                }

                if (hitRange >= (100 - avProb))
                {
                    newMax = (int)Math.Round(max - (max * 0.1D));
                    return (int)Math.Round(Math.Abs(newMax - newMax * (random.Next(0, 50) * 0.01D)), MidpointRounding.AwayFromZero);
                }

                newMax = (int)Math.Round(max - max * 0.5D);
                return (int)Math.Round(Math.Abs((newMax - (newMax * (random.Next(0, 50) * 0.01D)))), MidpointRounding.AwayFromZero);

                //return (int)Math.Round(Math.Abs(newMax - newMax * (random.Next(0, 95) * 0.01D)), MidpointRounding.AwayFromZero);
            }

            return 0;
        }


        private static float CalculateCastDamage(float defenderArmorPower, int rangedLevel, double power)
        {
            var rangeLvl = rangedLevel;
            var armour = defenderArmorPower;
            var rangeEquip = 15f;
            int armourRatio = (int)(60D + ((double)((rangeEquip * 3D) - armour) / 300D) * 40D);

            var randomValue = random.NextDouble();
            if (randomValue * 100f > armourRatio && randomValue <= 0.5)
            {
                return 0;
            }

            int max = (int)(((double)rangeLvl * 0.15D) + 0.85D + power);
            int peak = (int)(((double)max / 100D) * (double)armourRatio);
            int dip = (int)(((double)peak / 3D) * 2D);
            return RandomWeighted(0, dip, peak, max);
        }

        private static double ArrowPower(int arrowId)
        {
            return arrowId * 0.5f;
        }

        private static double MagicPower(int arrowId)
        {
            return arrowId * 0.5f;
        }

        public static int RandomWeighted(int low, int dip, int peak, int max)
        {
            int total = 0;
            int probability = 100;
            int[] probArray = new int[max + 1];
            for (int x = 0; x < probArray.Length; x++)
            {
                total += probArray[x] = probability;
                if (x < dip || x > peak)
                {
                    probability -= 3;
                }
                else
                {
                    probability += 3;
                }
            }
            int hit = random.Next(0, total);
            total = 0;
            for (int x = 0; x < probArray.Length; x++)
            {
                if (hit >= total && hit < (total + probArray[x]))
                {
                    return x;
                }
                total += probArray[x];
            }
            return 0;
        }

        private static int StyleBonus(int attackerCombatStyle, int skill)
        {
            var style = attackerCombatStyle;
            if (style == 0) return 1;
            return (skill == 0 && style == 2) || (skill == 1 && style == 3) || (skill == 2 && style == 1) ? 3 : 0;
        }
    }
}