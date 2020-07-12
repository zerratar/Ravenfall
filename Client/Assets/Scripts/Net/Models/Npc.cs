namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Npc
    {
        public int Id { get; set; }
        public int NpcId { get; set; }        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }
    }
}
