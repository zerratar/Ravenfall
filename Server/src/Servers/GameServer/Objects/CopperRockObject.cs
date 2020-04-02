using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Threading;

namespace RavenfallServer.Objects
{
    public class CopperRockObject : SceneObject
    {
        [Obsolete("Going to be removed")]
        public float HitChance { get; set; }
        public int RespawnMilliseconds { get; set; }
        public decimal Experience { get; set; }
        internal static CopperRockObject Create(
            ref int index,
            Vector3 position,
            decimal exp = 15,
            float hitChance = 0.5f,
            int respawnMs = 5000)
        {
            return new CopperRockObject
            {
                Id = Interlocked.Increment(ref index),
                ObjectId = 2,
                Position = position,
                Experience = exp,
                RespawnMilliseconds = respawnMs,
                HitChance = hitChance
            };
        }
    }
    
}
