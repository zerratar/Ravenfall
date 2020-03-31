using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public class SyntyAppearance : Entity<SyntyAppearance>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private Gender gender; public Gender Gender { get => gender; set => Set(ref gender, value); }
        private int hair; public int Hair { get => hair; set => Set(ref hair, value); }
        private int head; public int Head { get => head; set => Set(ref head, value); }
        private int eyebrows; public int Eyebrows { get => eyebrows; set => Set(ref eyebrows, value); }
        private int facialHair; public int FacialHair { get => facialHair; set => Set(ref facialHair, value); }
        private string skinColor; public string SkinColor { get => skinColor; set => Set(ref skinColor, value); }
        private string hairColor; public string HairColor { get => hairColor; set => Set(ref hairColor, value); }
        private string beardColor; public string BeardColor { get => beardColor; set => Set(ref beardColor, value); }
        private string eyeColor; public string EyeColor { get => eyeColor; set => Set(ref eyeColor, value); }
        private bool helmetVisible; public bool HelmetVisible { get => helmetVisible; set => Set(ref helmetVisible, value); }

        private string stubbleColor; public string StubbleColor { get => stubbleColor; set => Set(ref stubbleColor, value); }
        private string warPaintColor; public string WarPaintColor { get => warPaintColor; set => Set(ref warPaintColor, value); }

        //public ICollection<Character> Character { get; set; }
    }
}
