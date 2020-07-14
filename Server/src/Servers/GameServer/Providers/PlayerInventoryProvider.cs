
using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;

namespace RavenfallServer.Providers
{
    public class PlayerInventoryProvider : IPlayerInventoryProvider
    {
        private readonly ConcurrentDictionary<int, Inventory> inventories
            = new ConcurrentDictionary<int, Inventory>();

        private readonly IItemManager itemProvider;

        public PlayerInventoryProvider(IItemManager itemProvider)
        {
            this.itemProvider = itemProvider;
        }

        public Inventory GetInventory(int playerId)
        {
            if (inventories.TryGetValue(playerId, out var inventory))
                return inventory;

            inventory = inventories[playerId] = new Inventory();

            // Add Hatchet and Pickaxe as starting items
            inventory.AddItem(itemProvider.GetItemById(0), 1); // hatchet
            inventory.AddItem(itemProvider.GetItemById(1), 1); // pickaxe
            inventory.AddItem(itemProvider.GetItemById(7), 1); // fishing net

            return inventory;
        }
    }
}