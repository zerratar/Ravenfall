using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace RavenfallServer.Packets
{
    public class NpcHealthChange
    {
        public const short OpCode = 40;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        public int Delta { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        internal static NpcHealthChange Create(Npc npc, Player player, int damage, int health, int maxHealth)
        {
            return new NpcHealthChange
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
                Delta = damage,
                Health = health,
                MaxHealth = maxHealth
            };
        }
    }
}
