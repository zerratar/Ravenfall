using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class NpcTradeSellItemHandler : PlayerPacketHandler<NpcTradeSellItem>
    {
        private readonly ILogger logger;
        private readonly IPlayerInventoryProvider inventoryProvider;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameSessionManager sessionManager;

        public NpcTradeSellItemHandler(
            ILogger logger,
            IPlayerInventoryProvider inventoryProvider,
            IWorldProcessor worldProcessor,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.inventoryProvider = inventoryProvider;
            this.worldProcessor = worldProcessor;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(NpcTradeSellItem data, PlayerConnection connection)
        {
            logger.Debug("Player " + connection.Player.Id + " trying to sell item to NPC " + data.NpcServerId + " itemId: " + data.ItemId + " amount " + data.Amount);
            var session = sessionManager.Get(connection.Player);
            var npc = session.Npcs.Get(data.NpcServerId);
            if (npc == null) return;

            var inventory = inventoryProvider.GetInventory(connection.Player.Id);
            if (!inventory.HasItem(data.ItemId, data.Amount))
            {
                return;
            }

            worldProcessor.PlayerSellItem(connection.Player, npc, data.ItemId, data.Amount);
        }
    }
}
