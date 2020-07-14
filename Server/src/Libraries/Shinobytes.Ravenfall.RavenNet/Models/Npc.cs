namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Npc : WorldEntity
    {        
        public int NpcId { get; set; }
        public int RespawnTimeMs { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }
        public NpcAlignment Alignment { get; set; }
    }
}
