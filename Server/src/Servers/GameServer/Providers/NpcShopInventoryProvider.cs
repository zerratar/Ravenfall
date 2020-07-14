
using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;

namespace RavenfallServer.Providers
{
    public class NpcShopInventoryProvider : INpcShopInventoryProvider
    {
        private readonly ConcurrentDictionary<int, ShopInventory> inventories
          = new ConcurrentDictionary<int, ShopInventory>();

        private readonly IItemManager itemProvider;

        public NpcShopInventoryProvider(IItemManager itemProvider)
        {
            this.itemProvider = itemProvider;
        }

        public ShopInventory GetInventory(int npcServerId)
        {
            if (inventories.TryGetValue(npcServerId, out var inventory))
                return inventory;

            inventory = inventories[npcServerId] = new ShopInventory();

            // Add Hatchet and Pickaxe as starting items
            inventory.AddItem(itemProvider.GetItemById(0), 1, 10); // hatchet
            inventory.AddItem(itemProvider.GetItemById(1), 1, 10); // pickaxe
            inventory.AddItem(itemProvider.GetItemById(7), 1, 10); // fishing net
            inventory.AddItem(itemProvider.GetItemById(2), 0, 10); // copper ore
            inventory.AddItem(itemProvider.GetItemById(3), 0, 10); // logs

            return inventory;
        }
    }
}