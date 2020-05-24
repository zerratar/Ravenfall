using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private CharacterSelectionPanelUI characterSelection;
    [SerializeField] private TMPro.TMP_InputField inputName;
    [SerializeField] private UnityEngine.UI.Button btnCreate;


    [SerializeField] private AppearanceSlider face;
    [SerializeField] private AppearanceSlider hairstyle;
    [SerializeField] private AppearanceSlider eyebrows;
    [SerializeField] private AppearanceSlider facialHair;

    private Appearance appearance;

    // Start is called before the first frame update
    private void Start()
    {
        btnCreate.onClick.AddListener(new UnityEngine.Events.UnityAction(OnCreateClicked));
        face.ValueChanged += Face_ValueChanged;
        hairstyle.ValueChanged += Hairstyle_ValueChanged;
        eyebrows.ValueChanged += Eyebrows_ValueChanged;
        facialHair.ValueChanged += FacialHair_ValueChanged;

        GenerateCharacter();
        UpdateSliderMaxValues();
    }

    private void FacialHair_ValueChanged(object sender, EventArgs e)
    {
        appearance.FacialHair = facialHair.Value;
        UpdateAppearance();
    }

    private void Eyebrows_ValueChanged(object sender, EventArgs e)
    {
        appearance.Eyebrows = eyebrows.Value;
        UpdateAppearance();
    }

    private void Hairstyle_ValueChanged(object sender, EventArgs e)
    {
        appearance.Hair = hairstyle.Value;
        UpdateAppearance();
    }

    private void Face_ValueChanged(object sender, EventArgs e)
    {
        appearance.Head = face.Value;
        UpdateAppearance();
    }

    public void GenerateCharacter()
    {
        appearance = GetAppearanceData();
        UpdateAppearance();
    }

    public void SetGenderMale()
    {
        appearance.Gender = Gender.Male;
        UpdateAppearance();
        UpdateSliderMaxValues();
    }

    public void SetGenderFemale()
    {
        appearance.Gender = Gender.Female;
        UpdateAppearance();
        UpdateSliderMaxValues();
    }

    private void OnCreateClicked()
    {
        characterSelection.CreateCharacter(new Player
        {
            Name = inputName.text,
            Appearance = appearance
        });
        characterSelection.HideCharacterCreation();
    }

    private Appearance GetAppearanceData()
    {
        return GenerateRandomAppearance();
    }

    private void UpdateAppearance()
    {
        characterSelection.RemovePlayerPresentation();
        characterSelection.UpdatePlayerPresentation(new Player
        {
            Appearance = appearance
        });
    }

    private void UpdateSliderMaxValues()
    {
        var previewChar = characterSelection.GetPreviewCharacter();
        if (!previewChar) return;
        if (appearance == null) return;
        face.MaxValue = appearance.Gender == Gender.Male ? previewChar.MaleHeadCount : previewChar.FemaleHeadCount;
        eyebrows.MaxValue = appearance.Gender == Gender.Male ? previewChar.MaleEyebrowCount : previewChar.FemaleEyebrowCount;
        hairstyle.MaxValue = previewChar.HairCount;
        facialHair.MaxValue = appearance.Gender == Gender.Male ? previewChar.MaleFacialHairCount : 0;
    }


    private Appearance GenerateRandomAppearance()
    {
        var gender = Utility.Random<Gender>();
        var skinColor = Utility.Random("#d6b8ae");
        var hairColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
        var beardColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
        return new Appearance
        {
            Gender = gender,
            SkinColor = skinColor,
            HairColor = hairColor,
            BeardColor = beardColor,
            StubbleColor = skinColor,
            WarPaintColor = hairColor,
            EyeColor = Utility.Random("#000000", "#c0392b", "#2c3e50"),
            Eyebrows = Utility.Random(0, gender == Gender.Male ? 10 : 7),
            Hair = Utility.Random(0, 38),
            FacialHair = gender == Gender.Male ? Utility.Random(0, 18) : -1,
            Head = Utility.Random(0, 23),
            HelmetVisible = true
        };
    }

}

public static class Utility
{
    private static readonly System.Random random = new System.Random();

    public static T Random<T>(params T[] items)
    {
        return items
            .OrderBy(x => random.NextDouble()).First();
    }

    public static T Random<T>()
        where T : struct, IConvertible
    {
        return Enum
            .GetValues(typeof(T)).Cast<T>()
            .OrderBy(x => random.NextDouble()).First();
    }

    public static int Random(int min, int max)
    {
        return random.Next(min, max);
    }
}