using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerRemoveHandler : INetworkPacketHandler<PlayerRemove>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerRemoveHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerRemove data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("Player Remove, player id: " + data.PlayerId);
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            var player = new Player()
            {
                Id = data.PlayerId
            };

            playerHandler.Remove(player);
        }
    }
}
