using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Clan : Entity<Clan>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid userId; public Guid UserId { get => userId; set => Set(ref userId, value); }
        private string name; public string Name { get => name; set => Set(ref name, value); }
        private string logo; public string Logo { get => logo; set => Set(ref logo, value); }
    }
}