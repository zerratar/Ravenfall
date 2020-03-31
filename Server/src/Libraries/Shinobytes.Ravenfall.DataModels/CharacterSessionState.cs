using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public class CharacterSessionState
    {
        public Guid SessionId { get; set; }
        public Guid CharacterId { get; set; }
        public DateTime LastTaskUpdate { get; set; }
        public float SyncTime { get; set; }
        public ExpSkillGainCollection ExpGain { get; set; } = new ExpSkillGainCollection();
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Health { get; set; }
        public bool Compromised { get; set; }
    }
}