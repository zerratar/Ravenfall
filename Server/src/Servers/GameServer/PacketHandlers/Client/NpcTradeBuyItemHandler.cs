using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class NpcTradeBuyItemHandler : PlayerPacketHandler<NpcTradeBuyItem>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameSessionManager sessionManager;

        public NpcTradeBuyItemHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(NpcTradeBuyItem data, PlayerConnection connection)
        {
            logger.Debug("Player " + connection.Player.Id + " trying to buy item from NPC " + data.NpcServerId + " itemId: " + data.ItemId + " amount " + data.Amount);
            var session = sessionManager.Get(connection.Player);
            var npc = session.Npcs.Get(data.NpcServerId);
            if (npc == null) return;

            var inventory = session.Npcs.Inventories.GetInventory(data.NpcServerId);
            if (!inventory.HasItem(data.ItemId, data.Amount))
            {
                return;
            }

            worldProcessor.PlayerBuyItem(connection.Player, npc, data.ItemId, data.Amount);
        }
    }
}
