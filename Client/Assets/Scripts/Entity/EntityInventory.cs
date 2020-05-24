using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityInventory : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    private readonly List<InventoryItem> inventoryItems
        = new List<InventoryItem>();

    private void Start()
    {
        if (!itemManager) itemManager = FindObjectOfType<ItemManager>();
    }

    internal void AddItem(int itemId, int amount)
    {
        var item = itemManager.GetItemById(itemId);
        if (item == null) return;

        if (item.Stackable)
        {
            var stack = inventoryItems.FirstOrDefault(x => x.Item.Id == itemId);
            if (stack != null)
            {
                stack.Amount += amount;
                return;
            }

            inventoryItems.Add(new InventoryItem
            {
                Amount = amount,
                Item = item
            });
            return;
        }

        for (var i = 0; i < amount; ++i)
        {
            inventoryItems.Add(new InventoryItem
            {
                Amount = 1,
                Item = item
            });
        }

        Debug.Log("Item added to inventory: " + item.Name + " x" + amount);
    }

    internal void RemoveItem(int itemId, int amount)
    {
        var item = itemManager.GetItemById(itemId);
        if (item == null) return;
        if (item.Stackable)
        {
            var stack = inventoryItems.FirstOrDefault(x => x.Item.Id == itemId);
            if (stack != null)
            {
                stack.Amount -= amount;
                if (stack.Amount <= 0)
                {
                    inventoryItems.Remove(stack);
                }
                return;
            }
        }

        for (var i = 0; i < amount; ++i)
        {
            var targetItem = inventoryItems.FirstOrDefault(x => x.Item.Id == itemId);
            if (targetItem == null)
            {
                Debug.LogError("Trying to remove item from inventory that does not exist!!");
                return;
            }

            if (targetItem.Amount > 1)
            {
                Debug.LogError("Trying to remove item as non stackable but item in inventory stacked!!");
                return;
            }

            inventoryItems.Remove(targetItem);
        }
    }
}

