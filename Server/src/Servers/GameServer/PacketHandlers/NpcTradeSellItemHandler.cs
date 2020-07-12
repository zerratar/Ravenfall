using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class NpcTradeSellItemHandler : PlayerPacketHandler<NpcTradeSellItem>
    {
        private readonly ILogger logger;
        private readonly INpcProvider npcProvider;
        private readonly IPlayerInventoryProvider inventoryProvider;
        private readonly IWorldProcessor worldProcessor;

        public NpcTradeSellItemHandler(
            ILogger logger,
            INpcProvider npcProvider,
            IPlayerInventoryProvider inventoryProvider,
            IWorldProcessor worldProcessor)
        {
            this.logger = logger;
            this.npcProvider = npcProvider;
            this.inventoryProvider = inventoryProvider;
            this.worldProcessor = worldProcessor;
        }

        protected override void Handle(NpcTradeSellItem data, PlayerConnection connection)
        {
            logger.Debug("Player " + connection.Player.Id + " trying to sell item to NPC " + data.NpcServerId + " itemId: " + data.ItemId + " amount " + data.Amount);

            Npc npc = npcProvider.Get(data.NpcServerId);
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
