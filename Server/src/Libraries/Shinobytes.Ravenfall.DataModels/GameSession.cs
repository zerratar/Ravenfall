using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class GameSession : Entity<GameSession>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid userId; public Guid UserId { get => userId; set => Set(ref userId, value); }
        private DateTime started; public DateTime Started { get => started; set => Set(ref started, value); }
        private DateTime? stopped; public DateTime? Stopped { get => stopped; set => Set(ref stopped, value); }
        private DateTime? updated; public DateTime? Updated { get => updated; set => Set(ref updated, value); }
        private int status; public int Status { get => status; set => Set(ref status, value); }

        [Obsolete("Will be removed in the future, do not use. As we will not support local players")]
        private bool local; public bool Local { get => local; set => Set(ref local, value); }
        //private User _User; public User User { get => _User; set => Set(ref _User, value); }
        private long? revision; public long? Revision { get => revision; set => Set(ref revision, value); }
    }
}
