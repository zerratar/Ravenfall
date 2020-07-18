using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerLevelUp
    {
        public const short OpCode = 15;
        public int PlayerId { get; set; }
        public int Skill { get; set; }
        public int GainedLevels { get; set; }

        internal static PlayerLevelUp Create(Player player, EntityStat stat, int gainedLevels)
        {
            return new PlayerLevelUp
            {
                PlayerId = player.Id,
                Skill = stat.Index,
                GainedLevels = gainedLevels
            };
        }
    }
}
