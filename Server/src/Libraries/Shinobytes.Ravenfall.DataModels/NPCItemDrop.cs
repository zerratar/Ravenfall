using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public class NPCItemDrop : Entity<NPCItemDrop>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid npcId; public Guid NpcId { get => npcId; set => Set(ref npcId, value); }
        private Guid itemId; public Guid ItemId { get => itemId; set => Set(ref itemId, value); }
        private decimal dropChance; public decimal DropChance { get => dropChance; set => Set(ref dropChance, value); }
    }
}