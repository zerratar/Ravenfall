using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public class NPCSpawn : Entity<NPCSpawn>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }

        private Guid npcId; public Guid NpcId { get => npcId; set => Set(ref npcId, value); }
        private int x; public int X { get => x; set => Set(ref x, value); }
        private int y; public int Y { get => y; set => Set(ref y, value); }
        private int z; public int Z { get => z; set => Set(ref z, value); }
        private decimal respawnInterval; public decimal RespawnInterval { get => respawnInterval; set => Set(ref respawnInterval, value); }
    }
}