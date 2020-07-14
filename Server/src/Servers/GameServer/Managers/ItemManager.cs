using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class ItemManager : IItemManager
    {
        private readonly List<Item> entities = new List<Item>();

        public ItemManager()
        {
            entities.Add(new Item { Id = 0, Name = "Bronze Hatchet", Equippable = true, Type = 1 });
            entities.Add(new Item { Id = 1, Name = "Bronze Pickaxe", Equippable = true, Type = 2 });
            entities.Add(new Item { Id = 2, Name = "Copper Ore" });
            entities.Add(new Item { Id = 3, Name = "Log" });
            entities.Add(new Item { Id = 4, Name = "Shrimp" });
            entities.Add(new Item { Id = 5, Name = "Cooked Shrimp", Consumable = true });
            entities.Add(new Item { Id = 6, Name = "Burned Shrimp" });
            entities.Add(new Item { Id = 7, Name = "Fishing Net", Equippable = true, Type = 3 });
        }

        public Item GetItemById(int itemId)
        {
            return entities.FirstOrDefault(x => x.Id == itemId);
        }
    }
}
