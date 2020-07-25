using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerStatUpdated : EntityUpdated<Player>
    {
        public PlayerStatUpdated(
            Player entity,
            string skill,
            int level,
            decimal experience)
            : base(entity)
        {
            Skill = skill;
            Level = level;
            Experience = experience;
        }

        public string Skill { get; }
        public int Level { get; }
        public decimal Experience { get; }
    }
}
