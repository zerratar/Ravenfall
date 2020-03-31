using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Village : Entity<Village>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid userId; public Guid UserId { get => userId; set => Set(ref userId, value); }
        private string name; public string Name { get => name; set => Set(ref name, value); }
        private int level; public int Level { get => level; set => Set(ref level, value); }
        private long experience; public long Experience { get => experience; set => Set(ref experience, value); }
        private Guid resourcesId; public Guid ResourcesId { get => resourcesId; set => Set(ref resourcesId, value); }
    }
}