using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerInventoryUpdated : EntityUpdated<Player>
    {
        public PlayerInventoryUpdated(Player entity, int[] itemId, long[] amount)
            : base(entity)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public int[] ItemId { get; set; }
        public long[] Amount { get; set; }
    }
}
