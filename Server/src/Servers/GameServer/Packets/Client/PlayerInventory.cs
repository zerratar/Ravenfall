using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Packets
{
    public class PlayerInventory
    {
        public const short OpCode = 26;
        public int PlayerId { get; set; }
        public int[] ItemId { get; set; }
        public long[] Amount { get; set; }
        public long Coins { get; set; }
        public static PlayerInventory Create(Player player, IReadOnlyList<Inventory.InventoryItem> items)
        {
            var itemIds = new int[items.Count];
            var amounts = new long[items.Count];
            for (var i = 0; i < items.Count; ++i)
            {
                itemIds[i] = items[i].Item.Id;
                amounts[i] = items[i].Amount;
            }

            return new PlayerInventory
            {
                PlayerId = player.Id,
                Amount = amounts,
                ItemId = itemIds,
                Coins = player.Coins
            };
        }
    }
}
