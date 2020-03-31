using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerLeveledUp : EntityUpdated<Player>
    {
        public PlayerLeveledUp(
            Player entity,
            int skill,
            int gainedLevels)
            : base(entity)
        {
            Skill = skill;
            GainedLevels = gainedLevels;
        }

        public int Skill { get; }
        public int GainedLevels { get; }
    }
}
