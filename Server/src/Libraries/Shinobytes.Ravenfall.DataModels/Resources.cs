using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Resources : Entity<Resources>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private decimal wood; public decimal Wood { get => wood; set => Set(ref wood, value); }
        private decimal ore; public decimal Ore { get => ore; set => Set(ref ore, value); }
        private decimal fish; public decimal Fish { get => fish; set => Set(ref fish, value); }
        private decimal wheat; public decimal Wheat { get => wheat; set => Set(ref wheat, value); }
        private decimal magic; public decimal Magic { get => magic; set => Set(ref magic, value); }
        private decimal arrows; public decimal Arrows { get => arrows; set => Set(ref arrows, value); }
        private decimal coins; public decimal Coins { get => coins; set => Set(ref coins, value); }
    }
}
