using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Appearance : Entity<Appearance>
    {
        private Guid id;
        private Gender gender;
        private SkinColor skinColor;
        private HairColor hairColor;
        private HairColor browColor;
        private HairColor beardColor;
        private EyeColor eyeColor;
        private CostumeColor costumeColor;
        private int baseModelNumber;
        private int torsoModelNumber;
        private int bottomModelNumber;
        private int feetModelNumber;
        private int handModelNumber;
        private int beltModelNumber;
        private int eyesModelNumber;
        private int browsModelNumber;
        private int mouthModelNumber;
        private int maleHairModelNumber;
        private int femaleHairModelNumber;
        private int beardModelNumber;
        private int torsoMaterial;
        private int bottomMaterial;
        private int feetMaterial;
        private int handMaterial;
        private bool helmetVisible;

        public Guid Id
        {
            get => id;
            set => Set(ref id, value);
        }

        public Gender Gender
        {
            get => gender;
            set => Set(ref gender, value);
        }

        public SkinColor SkinColor
        {
            get => skinColor;
            set => Set(ref skinColor, value);
        }

        public HairColor HairColor
        {
            get => hairColor;
            set => Set(ref hairColor, value);
        }

        public HairColor BrowColor
        {
            get => browColor;
            set => Set(ref browColor, value);
        }

        public HairColor BeardColor
        {
            get => beardColor;
            set => Set(ref beardColor, value);
        }

        public EyeColor EyeColor
        {
            get => eyeColor;
            set => Set(ref eyeColor, value);
        }

        public CostumeColor CostumeColor
        {
            get => costumeColor;
            set => Set(ref costumeColor, value);
        }

        public int BaseModelNumber
        {
            get => baseModelNumber;
            set => Set(ref baseModelNumber, value);
        }

        public int TorsoModelNumber
        {
            get => torsoModelNumber;
            set => Set(ref torsoModelNumber, value);
        }

        public int BottomModelNumber
        {
            get => bottomModelNumber;
            set => Set(ref bottomModelNumber, value);
        }

        public int FeetModelNumber
        {
            get => feetModelNumber;
            set => Set(ref feetModelNumber, value);
        }

        public int HandModelNumber
        {
            get => handModelNumber;
            set => Set(ref handModelNumber, value);
        }

        public int BeltModelNumber
        {
            get => beltModelNumber;
            set => Set(ref beltModelNumber, value);
        }

        public int EyesModelNumber
        {
            get => eyesModelNumber;
            set => Set(ref eyesModelNumber, value);
        }

        public int BrowsModelNumber
        {
            get => browsModelNumber;
            set => Set(ref browsModelNumber, value);
        }

        public int MouthModelNumber
        {
            get => mouthModelNumber;
            set => Set(ref mouthModelNumber, value);
        }

        public int MaleHairModelNumber
        {
            get => maleHairModelNumber;
            set => Set(ref maleHairModelNumber, value);
        }

        public int FemaleHairModelNumber
        {
            get => femaleHairModelNumber;
            set => Set(ref femaleHairModelNumber, value);
        }

        public int BeardModelNumber
        {
            get => beardModelNumber;
            set => Set(ref beardModelNumber, value);
        }

        public int TorsoMaterial
        {
            get => torsoMaterial;
            set => Set(ref torsoMaterial, value);
        }

        public int BottomMaterial
        {
            get => bottomMaterial;
            set => Set(ref bottomMaterial, value);
        }

        public int FeetMaterial
        {
            get => feetMaterial;
            set => Set(ref feetMaterial, value);
        }

        public int HandMaterial
        {
            get => handMaterial;
            set => Set(ref handMaterial, value);
        }

        public bool HelmetVisible
        {
            get => helmetVisible;
            set => Set(ref helmetVisible, value);
        }
    }
}
