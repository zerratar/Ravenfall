using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerEquipmentStateUpdated : EntityUpdated<Player>
    {
        public PlayerEquipmentStateUpdated(
            Player entity,
            int itemId,
            bool equipped)
            : base(entity)
        {
            ItemId = itemId;
            Equipped = equipped;
        }

        public int ItemId { get; }
        public bool Equipped { get; }
    }
}
