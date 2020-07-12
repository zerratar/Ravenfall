using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcUpdate
    {
        public const short OpCode = 29;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}
