using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class ObjectUpdate
    {
        public const short OpCode = 11;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }
        public bool Static { get; set; }        
        internal static ObjectUpdate Create(WorldObject obj)
        {
            return new ObjectUpdate
            {
                ObjectServerId = obj.Id,
                ObjectId = obj.DisplayObjectId,
                Position = obj.Position,
                Static = obj.Static
            };
        }
    }
}
