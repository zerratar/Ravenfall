using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerEquipmentStateUpdate
    {
        public const short OpCode = 13;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public bool Equipped { get; set; }

        internal static PlayerEquipmentStateUpdate Create(Player player, Item item, bool equipped)
        {
            return new PlayerEquipmentStateUpdate
            {
                PlayerId = player.Id,
                ItemId = item.Id,
                Equipped = equipped
            };
        }
    }
}
