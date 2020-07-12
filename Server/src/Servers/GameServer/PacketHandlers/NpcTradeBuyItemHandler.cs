using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class NpcTradeBuyItemHandler : PlayerPacketHandler<NpcTradeBuyItem>
    {
        private readonly ILogger logger;
        private readonly INpcProvider npcProvider;
        private readonly INpcShopInventoryProvider shopInventoryProvider;
        private readonly IWorldProcessor worldProcessor;

        public NpcTradeBuyItemHandler(
            ILogger logger,
            INpcProvider npcProvider,
            INpcShopInventoryProvider shopInventoryProvider,
            IWorldProcessor worldProcessor)
        {
            this.logger = logger;
            this.npcProvider = npcProvider;
            this.shopInventoryProvider = shopInventoryProvider;
            this.worldProcessor = worldProcessor;
        }

        protected override void Handle(NpcTradeBuyItem data, PlayerConnection connection)
        {
            logger.Debug("Player " + connection.Player.Id + " trying to buy item from NPC " + data.NpcServerId + " itemId: " + data.ItemId + " amount " + data.Amount);

            Npc npc = npcProvider.Get(data.NpcServerId);
            if (npc == null) return;

            var inventory = shopInventoryProvider.GetInventory(data.NpcServerId);
            if (!inventory.HasItem(data.ItemId, data.Amount))
            {
                return;
            }

            worldProcessor.PlayerBuyItem(connection.Player, npc, data.ItemId, data.Amount);
        }
    }
}
