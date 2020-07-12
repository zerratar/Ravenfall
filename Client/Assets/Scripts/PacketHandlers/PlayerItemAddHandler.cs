using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerItemAddHandler : INetworkPacketHandler<PlayerItemAdd>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerItemAddHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerItemAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.Log("Add item to player (id: " + data.PlayerId + "), itemId: " + data.ItemId + ", amount: " + data.Amount);
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.PlayerItemAdd(data.PlayerId, data.ItemId, data.Amount);
        }
    }
}
