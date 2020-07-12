using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class OpenNpcTradeWindow : EntityUpdated<Player>
    {
        public OpenNpcTradeWindow(
         Player entity,
         int npcServerId,
         string shopName,
         int[] itemId,
         int[] itemPrice,
         int[] itemStock)
            : base(entity)
        {
            NpcServerId = npcServerId;
            ShopName = shopName;
            ItemId = itemId;
            ItemPrice = itemPrice;
            ItemStock = itemStock;
        }

        public int NpcServerId { get; }
        public int[] ItemId { get; }
        public int[] ItemPrice { get; }
        public int[] ItemStock { get; }
        public string ShopName { get; }
    }
}
