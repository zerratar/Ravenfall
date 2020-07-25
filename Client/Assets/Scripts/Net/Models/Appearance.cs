namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Appearance
    {
        public int Id { get; set; }
        public Gender Gender { get; set; }
        public int Hair { get; set; }
        public int Head { get; set; }
        public int Eyebrows { get; set; }
        public int FacialHair { get; set; }
        public string SkinColor { get; set; }
        public string HairColor { get; set; }
        public string BeardColor { get; set; }
        public string EyeColor { get; set; }
        public bool HelmetVisible { get; set; }
        public string StubbleColor { get; set; }
        public string WarPaintColor { get; set; }
    }

    public class Professions
    {
        public int Id { get; set; }
        public int Fishing { get; set; }
        public int Mining { get; set; }
        public int Crafting { get; set; }
        public int Cooking { get; set; }
        public int Woodcutting { get; set; }
        public int Farming { get; set; }
        public int Sailing { get; set; }
        public int Slayer { get; set; }

        public decimal FishingExp { get; set; }
        public decimal MiningExp { get; set; }
        public decimal CraftingExp { get; set; }
        public decimal CookingExp { get; set; }
        public decimal WoodcuttingExp { get; set; }
        public decimal FarmingExp { get; set; }
        public decimal SailingExp { get; set; }
        public decimal SlayerExp { get; set; }
    }

    public class Attributes
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Dexterity { get; set; }
        public int Agility { get; set; }
        public int Intellect { get; set; }
        public int Evasion { get; set; }
        public int Endurance { get; set; }

        public decimal EvasionExp { get; set; }
        public decimal IntellectExp { get; set; }
        public decimal AgilityExp { get; set; }
        public decimal DexterityExp { get; set; }
        public decimal DefenseExp { get; set; }
        public decimal StrengthExp { get; set; }
        public decimal HealthExp { get; set; }
        public decimal AttackExp { get; set; }
        public decimal EnduranceExp { get; set; }
    }
}
