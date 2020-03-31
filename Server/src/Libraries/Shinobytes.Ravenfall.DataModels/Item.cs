using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Item : Entity<Item>
    {
        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }
        private string name; public string Name { get => name; set => Set(ref name, value); }
        private int category; public int Category { get => category; set => Set(ref category, value); }
        private int type; public int Type { get => type; set => Set(ref type, value); }
        private int level; public int Level { get => level; set => Set(ref level, value); }
        private int weaponAim; public int WeaponAim { get => weaponAim; set => Set(ref weaponAim, value); }
        private int weaponPower; public int WeaponPower { get => weaponPower; set => Set(ref weaponPower, value); }
        private int armorPower; public int ArmorPower { get => armorPower; set => Set(ref armorPower, value); }
        private int requiredAttackLevel; public int RequiredAttackLevel { get => requiredAttackLevel; set => Set(ref requiredAttackLevel, value); }
        private int requiredDefenseLevel; public int RequiredDefenseLevel { get => requiredDefenseLevel; set => Set(ref requiredDefenseLevel, value); }
        private int material; public int Material { get => material; set => Set(ref material, value); }
        private string maleModelId; public string MaleModelId { get => maleModelId; set => Set(ref maleModelId, value); }
        private string femaleModelId; public string FemaleModelId { get => femaleModelId; set => Set(ref femaleModelId, value); }
        private string genericPrefab; public string GenericPrefab { get => genericPrefab; set => Set(ref genericPrefab, value); }
        private string malePrefab; public string MalePrefab { get => malePrefab; set => Set(ref malePrefab, value); }
        private string femalePrefab; public string FemalePrefab { get => femalePrefab; set => Set(ref femalePrefab, value); }
        private bool? isGenericModel; public bool? IsGenericModel { get => isGenericModel; set => Set(ref isGenericModel, value); }
        private bool? craftable; public bool? Craftable { get => craftable; set => Set(ref craftable, value); }
        private int requiredCraftingLevel; public int RequiredCraftingLevel { get => requiredCraftingLevel; set => Set(ref requiredCraftingLevel, value); }
        private long woodCost; public long WoodCost { get => woodCost; set => Set(ref woodCost, value); }
        private long oreCost; public long OreCost { get => oreCost; set => Set(ref oreCost, value); }
        private long shopBuyPrice; public long ShopBuyPrice { get => shopBuyPrice; set => Set(ref shopBuyPrice, value); }
        private long shopSellPrice; public long ShopSellPrice { get => shopSellPrice; set => Set(ref shopSellPrice, value); }
    }
}
