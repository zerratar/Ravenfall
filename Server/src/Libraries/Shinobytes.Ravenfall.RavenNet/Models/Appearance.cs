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
}