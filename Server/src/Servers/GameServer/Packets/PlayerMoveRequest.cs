using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerMoveRequest
    {
        public const short OpCode = 4;
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool Running { get; set; }
    }
}
