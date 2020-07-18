using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcRespawn
    {
        public const short OpCode = 42;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        internal static NpcRespawn Create(Npc npc, Player player)
        {
            return new NpcRespawn
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
            };
        }
    }
}
