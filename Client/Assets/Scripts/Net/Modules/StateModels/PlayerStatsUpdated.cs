using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerStatsUpdated : EntityUpdated<Player>
    {
        public PlayerStatsUpdated(
             Player entity,
             decimal[] experience,
             int[] effectiveLevel)
             : base(entity)
        {
            EffectiveLevel = effectiveLevel;
            Experience = experience;
        }

        public int Skill { get; }
        public int Level { get; }
        public int[] EffectiveLevel { get; }
        public decimal[] Experience { get; }
    }
}
