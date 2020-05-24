using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{

    public class PlayerEquipmentStateUpdateHandler : INetworkPacketHandler<PlayerEquipmentStateUpdate>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerEquipmentStateUpdateHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerEquipmentStateUpdate data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.SetEquimentState(data.PlayerId, data.ItemId, data.Equipped);
        }
    }
}
