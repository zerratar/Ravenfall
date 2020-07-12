using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerStatUpdate
    {
        public const short OpCode = 14;
        public int PlayerId { get; set; }
        public int Skill { get; set; }
        public int Level { get; set; }
        public int EffectiveLevel { get; set; }
        public decimal Experience { get; set; }

        internal static PlayerStatUpdate Create(Player player, EntityStat stat)
        {
            return new PlayerStatUpdate
            {
                PlayerId = player.Id,
                Skill = stat.Index,
                Experience = stat.Experience,
                Level = stat.Level,
                EffectiveLevel = stat.EffectiveLevel
            };
        }
    }
}
