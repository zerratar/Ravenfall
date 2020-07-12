using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class PlayerNpcActionRequestHandler : PlayerPacketHandler<PlayerNpcActionRequest>
    {
        private readonly ILogger logger;
        private readonly INpcProvider npcProvider;
        private readonly IWorldProcessor worldProcessor;
        private readonly IRavenConnectionProvider connectionProvider;

        public PlayerNpcActionRequestHandler(
            ILogger logger,
            INpcProvider npcProvider,
            IWorldProcessor worldProcessor,
            IRavenConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.npcProvider = npcProvider;
            this.worldProcessor = worldProcessor;
            this.connectionProvider = connectionProvider;
        }

        protected override void Handle(PlayerNpcActionRequest data, PlayerConnection connection)
        {
            // actionId 0 is examine and is client side
            if (data.ActionId == 0)
            {
                logger.Debug("Player sent examine action, ignoring");
                return;
            }

            logger.Debug("Player " + connection.Player.Id + " interacting with npc: " + data.NpcServerId + " action " + data.ActionId + " parameter " + data.ParameterId);

            Npc npc = npcProvider.Get(data.NpcServerId);
            if (npc == null) return;
            EntityAction action = npcProvider.GetAction(npc, data.ActionId);
            if (action == null) return;

            //// if we are already interacting with this object
            //// ignore it.
            //if (npcProvider.HasAcquiredObjectLock(npc, connection.Player))
            //{
            //    logger.Debug("Player is already interacting with object. Ignore");
            //    return;
            //}

            worldProcessor.PlayerNpcInteraction(connection.Player, npc, action, data.ParameterId);
        }
    }
}
