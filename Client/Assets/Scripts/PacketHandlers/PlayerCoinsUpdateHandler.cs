using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerCoinsUpdateHandler : INetworkPacketHandler<PlayerCoinsUpdate>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerCoinsUpdateHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerCoinsUpdate data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.Log("Player: " + data.PlayerId + ", received coins update: " + data.Coins);
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.SetPlayerInventory(data.PlayerId, data.Coins, null, null);
        }
    }
}
