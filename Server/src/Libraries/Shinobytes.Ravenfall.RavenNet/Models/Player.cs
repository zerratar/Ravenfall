namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player : WorldEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int CombatLevel { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }
        public long Coins { get; set; }
    }
}