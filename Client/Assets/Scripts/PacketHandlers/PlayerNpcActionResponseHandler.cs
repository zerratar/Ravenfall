using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using UnityEngine;
using ILogger = Shinobytes.Ravenfall.RavenNet.Core.ILogger;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerNpcActionResponseHandler : INetworkPacketHandler<PlayerNpcActionResponse>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerNpcActionResponseHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerNpcActionResponse data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("PlayerNpcActionResponseHandler::Handle");
            Debug.Log("PlayerNpcActionResponseHandler::Handle");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.NpcAction(data.PlayerId, data.NpcServerId, data.ActionId, data.ParameterId, data.Status);
        }
    }
}
