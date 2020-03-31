using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Skills : Entity<Skills>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private decimal attack; public decimal Attack { get => attack; set => Set(ref attack, value); }
        private decimal defense; public decimal Defense { get => defense; set => Set(ref defense, value); }
        private decimal strength; public decimal Strength { get => strength; set => Set(ref strength, value); }
        private decimal health; public decimal Health { get => health; set => Set(ref health, value); }
        private decimal magic; public decimal Magic { get => magic; set => Set(ref magic, value); }
        private decimal ranged; public decimal Ranged { get => ranged; set => Set(ref ranged, value); }
        private decimal woodcutting; public decimal Woodcutting { get => woodcutting; set => Set(ref woodcutting, value); }
        private decimal fishing; public decimal Fishing { get => fishing; set => Set(ref fishing, value); }
        private decimal mining; public decimal Mining { get => mining; set => Set(ref mining, value); }
        private decimal crafting; public decimal Crafting { get => crafting; set => Set(ref crafting, value); }
        private decimal cooking; public decimal Cooking { get => cooking; set => Set(ref cooking, value); }
        private decimal farming; public decimal Farming { get => farming; set => Set(ref farming, value); }
        private decimal slayer; public decimal Slayer { get => slayer; set => Set(ref slayer, value); }
        private decimal sailing; public decimal Sailing { get => sailing; set => Set(ref sailing, value); }
    }
}

