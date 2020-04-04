using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerItemRemoved : EntityUpdated<Player>
    {
        public PlayerItemRemoved(
            Player entity,
            int itemId,
            int amount)
            : base(entity)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public int ItemId { get; }
        public int Amount { get; }
    }
}
