using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class VillageHouse : Entity<VillageHouse>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid villageId; public Guid VillageId { get => villageId; set => Set(ref villageId, value); }
        private Guid? userId; public Guid? UserId { get => userId; set => Set(ref userId, value); }
        private int slot; public int Slot { get => slot; set => Set(ref slot, value); }
        private int type; public int Type { get => type; set => Set(ref type, value); }
        private DateTime created; public DateTime Created { get => created; set => Set(ref created, value); }
    }
}