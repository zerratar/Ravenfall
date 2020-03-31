using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerStatUpdated : EntityUpdated<Player>
    {
        public PlayerStatUpdated(
            Player entity,
            int skill,
            int level,
            int effectiveLevel,
            decimal experience)
            : base(entity)
        {
            Skill = skill;
            Level = level;
            EffectiveLevel = effectiveLevel;
            Experience = experience;
        }

        public int Skill { get; }
        public int Level { get; }
        public int EffectiveLevel { get; }
        public decimal Experience { get; }
    }
}
