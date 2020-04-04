using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Inventory
    {
        private readonly ConcurrentDictionary<int, InventoryItem> inventoryItems
            = new ConcurrentDictionary<int, InventoryItem>();

        public InventoryItem GetItem(int id)
        {
            return inventoryItems.TryGetValue(id, out var invItem) ? invItem : null;
        }

        public InventoryItem GetItemOfType(int interactItemType)
        {
            return inventoryItems.Values.FirstOrDefault(x => x.Item.Type == interactItemType);
        }

        public void EquipItem(Item item)
        {   
        }

        public void UnEquipItem(Item item)
        {         
        }

        public void AddItem(Item item, int amount)
        {
            if (inventoryItems.TryGetValue(item.Id, out var existing))
            {
                existing.Amount += amount;
            }
            else
            {
                inventoryItems[item.Id] = new InventoryItem
                {
                    Item = item,
                    Amount = amount
                };
            }
        }

        public void RemoveItem(Item item, int amount)
        {
            if (!inventoryItems.TryGetValue(item.Id, out var existing))
                return;

            existing.Amount -= amount;
        }

        public class InventoryItem
        {
            public Item Item { get; set; }
            public long Amount { get; set; }
        }
    }
}