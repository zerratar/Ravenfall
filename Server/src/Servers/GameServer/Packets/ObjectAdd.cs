using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class ObjectAdd
    {
        public const short OpCode = 9;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }

        internal static ObjectAdd Create(WorldObject obj)
        {
            return new ObjectAdd
            {
                ObjectServerId = obj.Id,
                ObjectId = obj.DisplayObjectId,
                Position = obj.Position
            };
        }
    }
}
