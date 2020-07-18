using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcMove
    {
        public const short OpCode = 30;
        public int ServerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool Running { get; set; }
    }
}
