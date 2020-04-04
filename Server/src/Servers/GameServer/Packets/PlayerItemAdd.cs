using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerItemAdd
    {
        public const short OpCode = 17;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }

        internal static PlayerItemAdd Create(Player player, Item item, int amount = 1)
        {
            return new PlayerItemAdd
            {
                PlayerId = player.Id,
                ItemId = item.Id,
                Amount = amount
            };
        }
    }
}
