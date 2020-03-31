using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class CharacterState : Entity<CharacterState>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private int health; public int Health { get => health; set => Set(ref health, value); }
        private string duelOpponent; public string DuelOpponent { get => duelOpponent; set => Set(ref duelOpponent, value); }
        private bool inRaid; public bool InRaid { get => inRaid; set => Set(ref inRaid, value); }
        private bool inArena; public bool InArena { get => inArena; set => Set(ref inArena, value); }
        private string task; public string Task { get => task; set => Set(ref task, value); }
        private string taskArgument; public string TaskArgument { get => taskArgument; set => Set(ref taskArgument, value); }
        private string island; public string Island { get => island; set => Set(ref island, value); }
        [Obsolete("Will be removed in the future")]
        private decimal? x; public decimal? X { get => x; set => Set(ref x, value); }
        [Obsolete("Will be removed in the future")]
        private decimal? y; public decimal? Y { get => y; set => Set(ref y, value); }
        [Obsolete("Will be removed in the future")]
        private decimal? z; public decimal? Z { get => z; set => Set(ref z, value); }
        //public ICollection<Character> Character { get; set; }
    }
}