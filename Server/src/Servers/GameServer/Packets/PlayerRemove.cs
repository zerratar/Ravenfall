using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerRemove
    {
        public const short OpCode = 3;
        public int PlayerId { get; set; }

        internal static PlayerRemove Create(Player player)
        {
            return new PlayerRemove()
            {
                PlayerId = player.Id
            };
        }
    }
}
