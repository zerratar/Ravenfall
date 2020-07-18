using GameServer.Processors;
using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class ChatMessagehandler : PlayerPacketHandler<ChatMessage>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;

        public ChatMessagehandler(
           ILogger logger,
           IWorldProcessor worldProcessor)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
        }

        protected override void Handle(ChatMessage data, PlayerConnection connection)
        {
            worldProcessor.SendChatMessage(connection.Player, data.ChannelId, data.Message);
        }
    }
}
