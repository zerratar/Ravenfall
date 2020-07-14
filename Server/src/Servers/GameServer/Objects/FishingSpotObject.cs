using Shinobytes.Ravenfall.RavenNet.Models;
using System.Threading;

namespace GameServer.Objects
{
    public class FishingSpotObject : WorldObject
    {
        internal static FishingSpotObject Create(
            ref int index,
            Vector3 position,
            int objectId = 3,
            decimal exp = 15,
            int respawnMs = 5000)
        {
            return new FishingSpotObject
            {
                Id = Interlocked.Increment(ref index),
                ObjectId = objectId,
                Position = position,
                InteractItemType = 3,
                Experience = exp,
                RespawnMilliseconds = respawnMs,
            };
        }
    }
}
