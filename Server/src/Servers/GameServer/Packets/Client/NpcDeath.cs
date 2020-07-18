using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcDeath
    {
        public const short OpCode = 41;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        internal static NpcDeath Create(Npc npc, Player player)
        {
            return new NpcDeath
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
            };
        }
    }
}
