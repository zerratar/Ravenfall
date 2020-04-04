using Shinobytes.Ravenfall.RavenNet.Models;
using System.Threading;

namespace RavenfallServer.Objects
{
    public class Chair : SceneObject
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
