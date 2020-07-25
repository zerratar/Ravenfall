using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityInventory : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private UIManager uiManager;

    private readonly List<InventoryItem> inventoryItems
        = new List<InventoryItem>();

    public long Coins { get; private set; }

    private void Start()
    {
        if (!itemManager) itemManager = FindObjectOfType<ItemManager>();
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
    }

    internal void SetItems(int[] itemId, long[] itemAmounts)
    {
        Debug.Log("EntityInventory::SetItems");

        inventoryItems.Clear();

        for (var i = 0; i < itemId.Length; ++i)
        {
            var id = itemId[i];
            var amount = itemAmounts[i];
            AddItem(id, amount, false);
        }

        uiManager.Inventory.SetInventoryItems(this.inventoryItems.ToArray());
    }

    internal ServerItem AddItem(int itemId, long amount, bool updateInventory = true)
    {
        try
        {
            var item = itemManager.GetItemById(itemId);
            if (item == null) return null;

            if (item.Stackable)
            {
                var stack = inventoryItems.FirstOrDefault(x => x.Item.Id == itemId);
                if (stack != null)
                {
                    stack.Amount += amount;
                    return item;
                }

                inventoryItems.Add(new InventoryItem
                {
                    Amount = amount,
                    Item = item
                });
                return item;
            }

            for (var i = 0; i < amount; ++i)
            {
                inventoryItems.Add(new InventoryItem
                {
                    Amount = 1,
                    Item = item
                });
            }

            return item;
        }
        finally
        {
            if (updateInventory)
            {
                uiManager.Inventory.SetInventoryItems(this.inventoryItems.ToArray());
            }
        }
    }

    internal bool HasItem(ServerItem item)
    {
        return inventoryItems.FirstOrDefault(x => x.Item.Id == item.Id) != null;
    }

    internal ServerItem RemoveItem(int itemId, long amount)
    {
        try
        {
            var item = itemManager.GetItemById(itemId);
            if (item == null) return item;
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
                    return item;
                }
            }

            for (var i = 0; i < amount; ++i)
            {
                var targetItem = inventoryItems.FirstOrDefault(x => x.Item.Id == itemId);
                if (targetItem == null)
                {
                    Debug.LogError("Trying to remove item from inventory that does not exist!!");
                    return item;
                }

                if (targetItem.Amount > 1)
                {
                    Debug.LogError("Trying to remove item as non stackable but item in inventory stacked!!");
                    return item;
                }

                inventoryItems.Remove(targetItem);
            }

            return item;
        }
        finally
        {
            uiManager.Inventory.SetInventoryItems(this.inventoryItems.ToArray());
        }
    }

    internal void SetCoins(long amount)
    {
        Coins = amount;
        uiManager.Inventory.SetCoins(amount);
    }
}

