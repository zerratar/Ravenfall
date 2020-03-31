using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class GameEvent : Entity<GameEvent>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid gameSessionId; public Guid GameSessionId { get => gameSessionId; set => Set(ref gameSessionId, value); }
        private int type; public int Type { get => type; set => Set(ref type, value); }
        private int revision; public int Revision { get => revision; set => Set(ref revision, value); }
        private string data; public string Data { get => data; set => Set(ref data, value); }
    }
}