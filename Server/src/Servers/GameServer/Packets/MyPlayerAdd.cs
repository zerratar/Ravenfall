using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace RavenfallServer.Packets
{
    public class MyPlayerAdd
    {
        public const short OpCode = 16;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public int[] EffectiveLevel { get; set; }
        public decimal[] Experience { get; set; }
        internal static MyPlayerAdd Create(Player player, IEnumerable<PlayerStat> stats)
        {
            return new MyPlayerAdd
            {
                PlayerId = player.Id,
                Name = player.Name,
                Experience = stats.Select(x => x.Experience).ToArray(),
                EffectiveLevel = stats.Select(x => x.EffectiveLevel).ToArray()
            };
        }
    }
}
