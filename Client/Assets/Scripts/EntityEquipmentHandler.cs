using MTAssets;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EntityEquipmentHandler : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private GameObject itemGameObject;

    [Header("Optimizer")]
    [SerializeField] private SkinnedMeshCombiner meshCombiner;

    /* Genderless Models */
    [Header("Generic Model Objects")]
    //[SerializeField] private GameObject[] hairCoverings;    
    [SerializeField] private GameObject[] faceCoverings;
    //[SerializeField] private GameObject[] headCoverings;

    [SerializeField] private GameObject[] hairs;
    //[SerializeField] private GameObject[] helmetAttachments;

    [SerializeField] private GameObject[] shoulderPadsRight;
    [SerializeField] private GameObject[] shoulderPadsLeft;

    [SerializeField] private GameObject[] elbowsRight;
    [SerializeField] private GameObject[] elbowsLeft;

    //[SerializeField] private GameObject[] hipsAttachments;

    [SerializeField] private GameObject[] kneeAttachmentsRight;
    [SerializeField] private GameObject[] kneeAttachmentsLeft;

    /* Male Models */
    [Header("Male Model Objects")]
    [SerializeField] private GameObject[] maleHeads;
    [SerializeField] private GameObject[] maleHelmets;

    [SerializeField] private GameObject[] maleEyebrows;
    [SerializeField] private GameObject[] maleFacialHairs;
    [SerializeField] private GameObject[] maleTorso;
    [SerializeField] private GameObject[] maleArmUpperRight;
    [SerializeField] private GameObject[] maleArmUpperLeft;

    [SerializeField] private GameObject[] maleArmLowerRight;
    [SerializeField] private GameObject[] maleArmLowerLeft;

    [SerializeField] private GameObject[] maleHandsRight;
    [SerializeField] private GameObject[] maleHandsLeft;
    [SerializeField] private GameObject[] maleHips;

    [SerializeField] private GameObject[] maleLegsRight;
    [SerializeField] private GameObject[] maleLegsLeft;

    /* Female Models */
    [Header("Female Model Objects")]
    [SerializeField] private GameObject[] femaleHeads;
    [SerializeField] private GameObject[] femaleHelmets;

    [SerializeField] private GameObject[] femaleEyebrows;
    [SerializeField] private GameObject[] femaleFacialHairs;
    [SerializeField] private GameObject[] femaleTorso;
    [SerializeField] private GameObject[] femaleArmUpperRight;
    [SerializeField] private GameObject[] femaleArmUpperLeft;

    [SerializeField] private GameObject[] femaleArmLowerRight;
    [SerializeField] private GameObject[] femaleArmLowerLeft;

    [SerializeField] private GameObject[] femaleHandsRight;
    [SerializeField] private GameObject[] femaleHandsLeft;
    [SerializeField] private GameObject[] femaleHips;

    [SerializeField] private GameObject[] femaleLegsRight;
    [SerializeField] private GameObject[] femaleLegsLeft;

    [Header("Character Setup")]
    public Gender Gender;
    public int Hair;
    public int Head = 0;
    public int Eyebrows = 0;
    public int FacialHair = 0;

    public int Shoulder = -1;
    public int Elbow = -1;
    public int Kneepad = -1;
    public int Helmet = -1;
    public int Torso = 0;
    public int ArmUpper = 0;
    public int ArmLower = 0;
    public int Hands = 0;
    public int Hips = 0;
    public int Legs = 0;

    public Color SkinColor;
    public Color HairColor;
    public Color BeardColor;
    public Color StubbleColor;
    //public Color EyebrowsColor;
    public Color WarPaintColor;
    public Color ScarColor;
    public Color EyeColor;

    public bool HelmetVisible;

    public int HairCount => hairs.Length;
    public int MaleHeadCount => maleHeads.Length;
    public int MaleFacialHairCount => maleFacialHairs.Length;
    public int MaleEyebrowCount => maleEyebrows.Length;
    public int FemaleHeadCount => femaleHeads.Length;
    public int FemaleEyebrowCount => femaleEyebrows.Length;


    private Dictionary<string, GameObject[]> modelObjects;

    private readonly Dictionary<EquipmentSlot, Transform> equipmentTransformLookup
        = new Dictionary<EquipmentSlot, Transform>();

    //private readonly List<NetworkItem> equippedItemObjects = new List<NetworkItem>();

    private readonly Dictionary<EquipmentSlot, NetworkItem> equippedItemObjects = new Dictionary<EquipmentSlot, NetworkItem>();

    private void Start()
    {
        if (!meshCombiner) meshCombiner = GetComponentInChildren<SkinnedMeshCombiner>();

        var hips = transform.Find("Character/Root/Hips");
        if (hips)
        {
            equipmentTransformLookup[EquipmentSlot.None] = null;
            equipmentTransformLookup[EquipmentSlot.Head] = hips.Find("Spine_01/Spine_02/Spine_03/Neck/Head");
            equipmentTransformLookup[EquipmentSlot.Chest] = hips.Find("Spine_01/Spine_02/Spine_03/");
            equipmentTransformLookup[EquipmentSlot.MainHand] = hips.Find("Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
            equipmentTransformLookup[EquipmentSlot.OffHand] = hips.Find("Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L");
        }

        if (!itemManager)
        {
            itemManager = FindObjectOfType<ItemManager>();
        }
    }

    public void SetAppearance(Appearance appearance, bool optimize = true)
    {
        if (appearance == null)
        {
            RandomizeCharacter();
            return;
        }

        ResetAppearance();

        var props = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name, x => x);
        foreach (var prop in appearance
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (props.TryGetValue(prop.Name, out var p))
            {
                var valueToSet = prop.GetValue(appearance);
                try
                {
                    if (p.FieldType == typeof(Color))
                    {
                        p.SetValue(this, GetColorFromHex(valueToSet?.ToString()));
                    }
                    else
                    {
                        p.SetValue(this, valueToSet);
                    }

                }
                catch (Exception exc)
                {
                    Debug.LogError(exc.ToString());
                }
            }
        }

        UpdateAppearance();
        if (optimize)
            Optimize();
    }

    public void UpdateAppearance()
    {
        ResetAppearance();
        var models = GetAllModels();
        var fields = GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.FieldType == typeof(int))
            .ToList();

        fields.ForEach(field =>
        {
            var items = models.Where(x =>
                x.Key.StartsWith(field.Name, StringComparison.OrdinalIgnoreCase) ||
                x.Key.StartsWith(Gender.ToString() + field.Name, StringComparison.OrdinalIgnoreCase));

            var index = (int)field.GetValue(this);
            if (index == -1) return;
            foreach (var item in items)
            {
                try
                {
                    // female facial hairs Kappa
                    if (item.Value.Length == 0)
                    {
                        continue;
                    }

                    if (index >= item.Value.Length && item.Key != nameof(femaleHeads) && item.Key != nameof(maleHeads))
                        continue;

                    if (item.Value.Length > 0)
                    {
                        index = Mathf.Min(item.Value.Length - 1, index);
                    }

                    if (item.Key == nameof(femaleEyebrows))
                    {

                    }

                    item.Value[index].SetActive(true);
                    var renderer = item.Value[index].GetComponent<SkinnedMeshRenderer>();
                    if (renderer)
                    {
                        //renderer.material =   //itemMaterials.Random();

                        if (item.Key == nameof(femaleHeads) || item.Key == nameof(maleHeads))
                        {
                            renderer.material.SetColor("_Color_Eyes", EyeColor);
                            renderer.material.SetColor("_Color_Skin", SkinColor);
                            //renderer.material.SetColor("_Color_Scar", ScarColor);
                            renderer.material.SetColor("_Color_Stubble", StubbleColor);
                            renderer.material.SetColor("_Color_BodyArt", WarPaintColor);
                        }
                        else if (item.Key == nameof(maleHeads) || item.Key == nameof(maleFacialHairs))
                        {
                            //renderer.material.SetColor("_Color_Stubble", BeardColor);
                            renderer.material.SetColor("_Color_Hair", BeardColor);
                        }
                        else if (item.Key == nameof(maleEyebrows) || item.Key == nameof(femaleEyebrows))
                        {
                            renderer.material.SetColor("_Color_Hair", HairColor);
                        }
                        else if (item.Key == nameof(hairs))
                        {
                            renderer.material.SetColor("_Color_Hair", HairColor);
                        }
                        // when unequipped, we need to update the skin color to match
                        // any other items should not have material changed or it will be instanced.
                        else if (index == 0)
                        {
                            renderer.material.SetColor("_Color_Skin", SkinColor);
                        }

                        //renderer.material.SetColor("_Color_Eyes", EyeColor);
                        //renderer.material.SetColor("_Color_Hair", HairColor);
                        //renderer.material.SetColor("_Color_Skin", SkinColor);
                        //renderer.material.SetColor("_Color_Stubble", BeardColor);
                    }
                }
                catch
                {
                    Debug.LogError($"({field.Name}) {item.Key}[{index}] out of bounds, {item.Key}.Length = {item.Value.Length}");
                }
            }
        });
    }
    public void ResetAppearance()
    {
        var allModels = GetAll();
        foreach (var model in allModels)
        {
            if (model != null)
                model.SetActive(false);
        }

        if (!meshCombiner) return;
        if (meshCombiner.isMeshesCombineds)
            meshCombiner.UndoCombineMeshes();
    }

    public void Optimize(Action afterUndo = null)
    {
        if (!meshCombiner || !this.gameObject.activeInHierarchy || !this.gameObject.activeSelf)
            return;

        int meshLayer = -1;
        if (transform.Find("Combined Mesh"))
        {
            meshCombiner.UndoCombineMeshes();
        }

        afterUndo?.Invoke();
        meshCombiner.meshesToIgnore.Clear();
        meshCombiner.CombineMeshes();
    }

    internal void SetEquipmentState(int itemId, bool equipped)
    {
        if (!itemManager)
        {
            Debug.LogError("ItemManager not set!!! Unable to update equipment state");
            return;
        }

        var item = itemManager.GetItemById(itemId);
        if (!item)
        {
            Debug.LogError("Unable to set equipment state, no items with ID " + itemId + " was found.");
            return;
        }

        var bone = GetEquipmentSlotTransform(item.EquipmentSlot);
        if (!bone)
        {
            Debug.LogError("No equipment transform found for slot: " + item.EquipmentSlot);
            return;
        }

        if (equipped)
        {
            var equippedItem = equippedItemObjects.FirstOrDefault(x => x.Value.ServerId == itemId).Value;
            if (equippedItem)
            {
                return;
            }

            var itemObject = Instantiate(itemGameObject, bone).GetComponent<NetworkItem>();
            itemObject.Data = item;
            itemObject.ServerId = item.Id;
            itemObject.Model = Instantiate(item.Model, itemObject.transform);
            equippedItemObjects[item.EquipmentSlot] = itemObject;
        }
        else
        {
            var equippedItem = equippedItemObjects.FirstOrDefault(x => x.Value.ServerId == itemId).Value;
            if (!equippedItem)
            {
                Debug.LogError("Unable to unequip target item, no equipped items with ID " + itemId + " was found.");
                return;
            }

            Destroy(equippedItem.gameObject);
            equippedItemObjects.Remove(item.EquipmentSlot);
        }
    }

    private GameObject[] GetAll()
    {
        return GetAllModels().SelectMany(x => x.Value).ToArray();
    }

    private IReadOnlyDictionary<string, GameObject[]> GetAllModels()
    {
        if (modelObjects == null)
        {
            modelObjects = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.FieldType == typeof(GameObject[]))
                .Select(x => new { Key = x.Name, Value = x.GetValue(this) as GameObject[] })
                .ToDictionary(x => x.Key, x => x.Value);
        }

        return modelObjects;
    }

    private void RandomizeCharacter()
    {
        Gender = Enum.GetValues(typeof(Gender)).Cast<Gender>().Random();
        var models = GetAllModels();
        var fields = GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.FieldType == typeof(int))
            .ToList();

        EyeColor = RandomColor();
        HairColor = RandomColor();
        BeardColor = RandomColor();
        SkinColor = RandomColor();

        HelmetVisible = RandomBool();

        fields.ForEach(field =>
        {
            var item = models
            .FirstOrDefault(x => x.Key.StartsWith(field.Name, StringComparison.OrdinalIgnoreCase) || x.Key.StartsWith(Gender.ToString() + field.Name, StringComparison.OrdinalIgnoreCase));

            if (item.Value != null && item.Value.Length > 0)
            {
                field.SetValue(this, item.Value.RandomIndex());
            }
        });

        UpdateAppearance();
        ToggleHelmVisibility();
    }

    public void ToggleHelmVisibility()
    {
        HelmetVisible = !HelmetVisible;
        Optimize(UpdateHelmetVisibility);
    }

    private void UpdateHelmetVisibility()
    {
        var helmets = Gender == Gender.Female ? femaleHelmets : maleHelmets;

        var equippedHelmet = helmets.FirstOrDefault(x => x.activeSelf);

        if (!equippedHelmet)
        {
            return;
        }

        if (Gender == Gender.Male)
        {
            Head = Math.Min(Head, maleHeads.Length - 1);
            //FacialHair = Math.Min(FacialHair, maleFacialHairs.Length - 1);
            //Eyebrows = Math.Min(Eyebrows, maleEyebrows.Length - 1);

            if (FacialHair < maleFacialHairs.Length)
                maleFacialHairs[FacialHair].gameObject.SetActive(!HelmetVisible);

            maleHeads[Head].gameObject.SetActive(!HelmetVisible);

            if (Eyebrows < maleEyebrows.Length)
                maleEyebrows[Eyebrows].gameObject.SetActive(!HelmetVisible);
        }
        else
        {
            Head = Math.Min(Head, femaleHeads.Length - 1);
            //Eyebrows = Math.Min(Eyebrows, femaleEyebrows.Length - 1);

            femaleHeads[Head].gameObject.SetActive(!HelmetVisible);
            if (Eyebrows < femaleEyebrows.Length)
                femaleEyebrows[Eyebrows].gameObject.SetActive(!HelmetVisible);
        }

        if (Hair < hairs.Length)
        {
            hairs[Hair].gameObject.SetActive(!HelmetVisible);
        }

        equippedHelmet.SetActive(HelmetVisible);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Transform GetEquipmentSlotTransform(EquipmentSlot eq)
    {
        if (equipmentTransformLookup.TryGetValue(eq, out var t)) return t;
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Color GetColorFromHex(string value)
    {
        ColorUtility.TryParseHtmlString(value, out var color);
        return color;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Color RandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool RandomBool()
    {
        return UnityEngine.Random.value >= 0.5;
    }
}
