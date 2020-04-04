using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerItemRemove
    {
        public const short OpCode = 18;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }

        internal static PlayerItemRemove Create(Player player, Item item, int amount = 1)
        {
            return new PlayerItemRemove
            {
                PlayerId = player.Id,
                ItemId = item.Id,
                Amount = amount
            };
        }
    }
}
