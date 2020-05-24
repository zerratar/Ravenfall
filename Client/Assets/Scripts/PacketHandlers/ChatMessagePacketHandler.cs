using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class ChatMessagePacketHandler : INetworkPacketHandler<ChatMessage>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public ChatMessagePacketHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(ChatMessage data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var chatHandler = moduleManager.GetModule<ChatMessageHandler>();
            chatHandler.AddChatMessage(data);
        }
    }
}
