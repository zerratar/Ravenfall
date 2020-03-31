namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool IsMe { get; set; }
    }
}
