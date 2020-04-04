namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Stackable { get; set; }
        public bool Equippable { get; set; }
        public bool Consumable { get; set; }
        public int Tier { get; set; }
        public int Type { get; set; }
    }
}
