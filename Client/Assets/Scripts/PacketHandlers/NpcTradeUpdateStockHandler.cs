using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcTradeUpdateStockHandler : INetworkPacketHandler<NpcTradeUpdateStock>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcTradeUpdateStockHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcTradeUpdateStock data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.LogWarning("Update stock!!");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.OpenTradeWindow(data.PlayerId, data.NpcServerId, null, data.ItemId, data.ItemPrice, data.ItemStock);
        }
    }
}
