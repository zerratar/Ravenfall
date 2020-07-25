namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool IsMe { get; set; }
        public bool Running { get; set; }
        public Appearance Appearance { get; set; }
        public Attributes Attributes { get; set; }
        public Professions Professions { get; set; }
        public SessionInfo Session { get; set; }
    }

    public class SessionInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
