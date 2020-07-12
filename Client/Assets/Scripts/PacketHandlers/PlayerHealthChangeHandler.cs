using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerHealthChangeHandler : INetworkPacketHandler<PlayerHealthChange>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerHealthChangeHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerHealthChange data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.UpdateHealth(data.TargetPlayerId, data.Health, data.MaxHealth, data.Delta);
        }
    }
}
