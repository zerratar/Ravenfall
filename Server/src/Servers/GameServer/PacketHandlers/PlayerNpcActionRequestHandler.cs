using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class PlayerNpcActionRequestHandler : PlayerPacketHandler<PlayerNpcActionRequest>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameSessionManager sessionManager;

        public PlayerNpcActionRequestHandler(
            ILogger logger,
            IWorldProcessor worldProcessor, 
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(PlayerNpcActionRequest data, PlayerConnection connection)
        {
            // actionId 0 is examine and is client side
            if (data.ActionId == 0)
            {
                logger.Debug("Player sent examine action, ignoring");
                return;
            }

            var session = sessionManager.Get(connection.Player);

            logger.Debug("Player " + connection.Player.Id + " interacting with npc: " + data.NpcServerId + " action " + data.ActionId + " parameter " + data.ParameterId);

            var npc = session.Npcs.Get(data.NpcServerId);
            if (npc == null) return;
            var action = session.Npcs.GetAction(npc, data.ActionId);
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
