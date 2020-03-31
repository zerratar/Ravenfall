using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using UnityEngine;
using ILogger = Shinobytes.Ravenfall.RavenNet.Core.ILogger;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerObjectActionResponseHandler : INetworkPacketHandler<PlayerObjectActionResponse>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerObjectActionResponseHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerObjectActionResponse data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("PlayerObjectActionResponseHandler::Handle");
            Debug.Log("PlayerObjectActionResponseHandler::Handle");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.Action(data.PlayerId, data.ObjectServerId, data.ActionId, data.ParameterId, data.Status);
        }
    }
}
