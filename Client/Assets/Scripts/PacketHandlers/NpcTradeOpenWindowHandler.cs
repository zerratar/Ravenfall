using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcTradeOpenWindowHandler : INetworkPacketHandler<NpcTradeOpenWindow>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcTradeOpenWindowHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcTradeOpenWindow data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.LogWarning("Open Trade window!!");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.OpenTradeWindow(data.PlayerId, data.NpcServerId, data.ShopName, data.ItemId, data.ItemPrice, data.ItemStock);
        }
    }
}
