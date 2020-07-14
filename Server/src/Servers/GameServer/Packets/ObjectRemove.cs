using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class ObjectRemove
    {
        public const short OpCode = 10;
        public int ObjectServerId { get; set; }

        internal static ObjectRemove Create(WorldObject obj)
        {
            return new ObjectRemove
            {
                ObjectServerId = obj.Id
            };
        }
    }
}
