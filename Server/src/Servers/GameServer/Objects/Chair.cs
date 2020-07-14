using Shinobytes.Ravenfall.RavenNet.Models;
using System.Threading;

namespace GameServer.Objects
{
    public class Chair : WorldObject
    {
        internal static Chair Create(
            ref int index,
            Vector3 position)
        {
            return new Chair
            {
                Id = Interlocked.Increment(ref index),
                ObjectId = 1000,
                Position = position,
            };
        }
    }
}
