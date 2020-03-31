using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using UnityEngine;
using ILogger = Shinobytes.Ravenfall.RavenNet.Core.ILogger;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerMoveResponseHandler : INetworkPacketHandler<PlayerMoveResponse>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerMoveResponseHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }
        public void Handle(PlayerMoveResponse data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("PlayerMoveResponseHandler::Handle");
            Debug.Log("PlayerMoveResponseHandler::Handle");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.Move(data.PlayerId, data.Position, data.Destination);
        }
    }
}
