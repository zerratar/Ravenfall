using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerPositionUpdate
    {
        public const short OpCode = 6;
        public Vector3 Position { get; set; }
    }
}
