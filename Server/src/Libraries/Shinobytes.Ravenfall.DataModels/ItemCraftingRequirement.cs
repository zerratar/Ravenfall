using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class ItemCraftingRequirement : Entity<ItemCraftingRequirement>
    {
        private Guid _Id; public Guid Id { get => _Id; set => Set(ref _Id, value); }
        private Guid _ItemId; public Guid ItemId { get => _ItemId; set => Set(ref _ItemId, value); }
        private Guid _ResourceItemId; public Guid ResourceItemId { get => _ResourceItemId; set => Set(ref _ResourceItemId, value); }
        private int _Amount; public int Amount { get => _Amount; set => Set(ref _Amount, value); }
    }
}
