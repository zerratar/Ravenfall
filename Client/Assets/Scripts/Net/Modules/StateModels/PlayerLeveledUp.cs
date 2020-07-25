using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerLeveledUp : EntityUpdated<Player>
    {
        public PlayerLeveledUp(
            Player entity,
            string skill,
            int gainedLevels)
            : base(entity)
        {
            Skill = skill;
            GainedLevels = gainedLevels;
        }

        public string Skill { get; }
        public int GainedLevels { get; }
    }
}
