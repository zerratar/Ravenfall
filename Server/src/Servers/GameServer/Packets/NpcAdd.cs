using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcAdd
    {
        public const short OpCode = 27;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }

        internal static NpcAdd Create(Npc obj)
        {
            return new NpcAdd
            {
                ServerId = obj.Id,
                NpcId = obj.NpcId,
#warning add health and max health for npc
                Position = obj.Position,
                Rotation = obj.Rotation,
                Destination = obj.Destination
            };
        }
    }
}
