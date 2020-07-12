using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class ShopInventory
    {
        private readonly ConcurrentDictionary<int, ShopInventoryItem> items
            = new ConcurrentDictionary<int, ShopInventoryItem>();

        public IReadOnlyList<ShopInventoryItem> Items => items.Values.ToList();

        public ShopInventoryItem GetItem(int id)
        {
            return items.TryGetValue(id, out var invItem) ? invItem : null;
        }

        public bool HasItem(int itemId, int amount)
        {
            var item = GetItem(itemId);
            return item != null && item.Amount >= amount;
        }

        public bool TryRemoveItem(Item item, int amount)
        {
            var i = GetItem(item.Id);
            if (i == null || i.Amount < amount)
                return false;

            RemoveItem(item, amount);
            return true;
        }

        public void AddItem(Item item, int amount, int price)
        {
            if (items.TryGetValue(item.Id, out var existing))
            {
                existing.Amount += amount;
            }
            else
            {
                items[item.Id] = new ShopInventoryItem
                {
                    Item = item,
                    Amount = amount,
                    Price = price,
                };
            }
        }

        public void RemoveItem(Item item, int amount)
        {
            if (!items.TryGetValue(item.Id, out var existing))
                return;

            existing.Amount -= amount;
        }

        public class ShopInventoryItem
        {
            public Item Item { get; set; }
            public int Amount { get; set; }
            public int Price { get; set; }
        }
    }
}