using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EntityEquipmentHandler : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private GameObject itemGameObject;

    private readonly Dictionary<EquipmentSlot, Transform> equipmentTransformLookup
        = new Dictionary<EquipmentSlot, Transform>();

    private readonly List<NetworkItem> equippedItemObjects = new List<NetworkItem>();

    private void Start()
    {
        var hips = transform.Find("Root");
        if (hips)
        {
            equipmentTransformLookup[EquipmentSlot.None] = null;
            //equipmentTransformLookup[EquipmentSlot.Head] = hips.Find("Spine_01/Spine_02/Spine_03/Neck/Head");
            //equipmentTransformLookup[EquipmentSlot.Chest] = hips.Find("Spine_01/Spine_02/Spine_03/");
            equipmentTransformLookup[EquipmentSlot.MainHand] = hips.Find("RightArm/RightHand");
            //equipmentTransformLookup[EquipmentSlot.OffHand] = hips.Find("Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L");
        }

        if (!itemManager)
        {
            itemManager = FindObjectOfType<ItemManager>();
        }
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
            var equippedItem = equippedItemObjects.FirstOrDefault(x => x.ServerId == itemId);
            if (equippedItem)
            {
                return;
            }

            var itemObject = Instantiate(itemGameObject, bone).GetComponent<NetworkItem>();
            itemObject.Data = item;
            itemObject.ServerId = item.Id;
            itemObject.Model = Instantiate(item.Model, itemObject.transform);
            equippedItemObjects.Add(itemObject);
        }
        else
        {
            var equippedItem = equippedItemObjects.FirstOrDefault(x => x.ServerId == itemId);
            if (!equippedItem)
            {
                Debug.LogError("Unable to unequip target item, no equipped items with ID " + itemId + " was found.");
                return;
            }

            Destroy(equippedItem.gameObject);
            equippedItemObjects.Remove(equippedItem);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Transform GetEquipmentSlotTransform(EquipmentSlot eq)
    {
        if (equipmentTransformLookup.TryGetValue(eq, out var t)) return t;
        return null;
    }
}
