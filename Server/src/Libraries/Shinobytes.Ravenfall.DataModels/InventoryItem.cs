using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class InventoryItem : Entity<InventoryItem>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid characterId; public Guid CharacterId { get => characterId; set => Set(ref characterId, value); }
        private Guid itemId; public Guid ItemId { get => itemId; set => Set(ref itemId, value); }
        private long? amount; public long? Amount { get => amount; set => Set(ref amount, value); }
        private bool equipped; public bool Equipped { get => equipped; set => Set(ref equipped, value); }
    }
}
