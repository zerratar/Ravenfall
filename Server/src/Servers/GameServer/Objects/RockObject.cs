using Shinobytes.Ravenfall.RavenNet.Models;
using System.Threading;

namespace GameServer.Objects
{
    public class RockObject : WorldObject
    {
        internal static RockObject Create(
            ref int index,
            Vector3 position,
            int objectId = 2,
            decimal exp = 15,
            int respawnMs = 5000)
        {
            return new RockObject
            {
                Id = Interlocked.Increment(ref index),
                ObjectId = objectId,
                Position = position,
                InteractItemType = 2,
                Experience = exp,
                RespawnMilliseconds = respawnMs,
            };
        }
    }
}
