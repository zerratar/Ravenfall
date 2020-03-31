using RavenfallServer.Items;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace RavenfallServer.Providers
{
    public class ItemProvider : IItemProvider
    {
        private readonly List<Item> entities = new List<Item>();     

        public ItemProvider()
        {
            entities.Add(new BronzeHatchet());
        }

        public Item GetItemById(int itemId)
        {
            return entities.FirstOrDefault(x => x.Id == itemId);
        }
    }
}
