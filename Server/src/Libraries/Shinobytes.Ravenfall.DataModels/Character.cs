using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Character : Entity<Character>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Guid userId; public Guid UserId { get => userId; set => Set(ref userId, value); }
        private Guid appearanceId; public Guid AppearanceId { get => appearanceId; set => Set(ref appearanceId, value); }
        private Guid? syntyAppearanceId; public Guid? SyntyAppearanceId { get => syntyAppearanceId; set => Set(ref syntyAppearanceId, value); }
        private Guid skillsId; public Guid SkillsId { get => skillsId; set => Set(ref skillsId, value); }
        private Guid statisticsId; public Guid StatisticsId { get => statisticsId; set => Set(ref statisticsId, value); }
        private Guid resourcesId; public Guid ResourcesId { get => resourcesId; set => Set(ref resourcesId, value); }
        private Guid? stateId; public Guid? StateId { get => stateId; set => Set(ref stateId, value); }
        private bool local; public bool Local { get => local; set => Set(ref local, value); }
        private Guid originUserId; public Guid OriginUserId { get => originUserId; set => Set(ref originUserId, value); }
        private DateTime created; public DateTime Created { get => created; set => Set(ref created, value); }
        private string name; public string Name { get => name; set => Set(ref name, value); }
        private int? revision; public int? Revision { get => revision; set => Set(ref revision, value); }
        private Guid? userIdLock; public Guid? UserIdLock { get => userIdLock; set => Set(ref userIdLock, value); }
        private DateTime? lastUsed; public DateTime? LastUsed { get => lastUsed; set => Set(ref lastUsed, value); }
        private Guid? clanId; public Guid? ClanId { get => clanId; set => Set(ref clanId, value); }
    }
}