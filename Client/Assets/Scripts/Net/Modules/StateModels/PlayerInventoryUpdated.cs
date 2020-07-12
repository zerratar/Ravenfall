using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerInventoryUpdated : EntityUpdated<Player>
    {
        public PlayerInventoryUpdated(Player entity, long coins, int[] itemId, long[] amount)
            : base(entity)
        {
            ItemId = itemId;
            Amount = amount;
            Coins = coins;
        }

        public int[] ItemId { get; set; }
        public long[] Amount { get; set; }
        public long Coins { get; set; }
    }
}
