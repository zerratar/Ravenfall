using Shinobytes.Ravenfall.Core;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class EntityStat
    {
        private static volatile int index;

        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Experience { get; set; }
        public int Level { get; set; }
        public int EffectiveLevel { get; set; }

        public int AddExperience(decimal amount)
        {
            var delta = GameMath.ExperienceToLevel(Experience) - this.Level;
            this.Experience += amount;
            this.Level += delta;
            this.EffectiveLevel += delta;
            return delta;
        }

        public static EntityStat Create(int index, string name, int level = 1, decimal experience = 0)
        {
            var levelExp = GameMath.LevelToExperience(level);
            return new EntityStat
            {
                Index = index,
                Id = index++,
                Name = name,
                Level = level,
                EffectiveLevel = level,
                Experience = experience > 0 ? experience : levelExp
            };
        }
    }
}
