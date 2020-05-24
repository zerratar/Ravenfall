using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{

    public class ChatMessage
    {
        public const short OpCode = 25;
        public int ChannelId { get; set; }
        public int PlayerId { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }

        public static ChatMessage Create(Player player, int channel, string message)
        {
            return new ChatMessage
            {
                ChannelId = channel,
                Message = message,
                PlayerId = player.Id,
                Sender = player.Name
            };
        }
    }
}
