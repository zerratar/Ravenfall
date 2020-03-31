using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public class NPCState
    {
        public Guid NpcId { get; set; }
        public Guid InstanceId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Health { get; set; }
    }
}