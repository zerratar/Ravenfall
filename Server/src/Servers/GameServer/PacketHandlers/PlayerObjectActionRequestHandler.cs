using System;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class PlayerObjectActionRequestHandler : PlayerPacketHandler<PlayerObjectActionRequest>
    {
        private readonly ILogger logger;
        private readonly IObjectProvider objectProvider;
        private readonly IWorldProcessor worldProcessor;
        private readonly IRavenConnectionProvider connectionProvider;

        public PlayerObjectActionRequestHandler(
            ILogger logger,
            IObjectProvider objectProvider,
            IWorldProcessor worldProcessor,
            IRavenConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.objectProvider = objectProvider;
            this.worldProcessor = worldProcessor;
            this.connectionProvider = connectionProvider;
        }
        protected override void Handle(PlayerObjectActionRequest data, PlayerConnection connection)
        {
            // actionId 0 is examine and is client side
            if (data.ActionId == 0)
            {
                logger.Debug("Player sent examine action, ignoring");
                return;
            }

            logger.Debug("Player " + connection.Player.Id + " interacting with object: " + data.ObjectServerId + " action " + data.ActionId + " parameter " + data.ParameterId);

            var serverObject = objectProvider.Get(data.ObjectServerId);
            if (serverObject == null) return;
            var action = objectProvider.GetAction(serverObject, data.ActionId);
            if (action == null) return;
            
            worldProcessor.PlayerObjectInteraction(connection.Player, serverObject, action, data.ParameterId);
        }
    }
}
