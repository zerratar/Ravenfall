using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Threading;

namespace GameServer.Objects
{
    public class TreeObject : Shinobytes.Ravenfall.RavenNet.Models.WorldObject
    {
        internal static TreeObject Create(
            ref int index,
            Vector3 position,
            int objectId = 0,
            decimal exp = 15,
            int respawnMs = 5000)
        {
            return new TreeObject
            {
                Id = Interlocked.Increment(ref index),
                ObjectId = objectId,
                DisplayObjectId = objectId,
                Position = position,
                Experience = exp,
                InteractItemType = 1,
                RespawnMilliseconds = respawnMs,
            };
        }
    }
}
