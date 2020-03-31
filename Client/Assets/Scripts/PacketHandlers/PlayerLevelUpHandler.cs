using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerLevelUpHandler : INetworkPacketHandler<PlayerLevelUp>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerLevelUpHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerLevelUp data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.PlayerLevelUp(data.PlayerId, data.Skill, data.GainedLevels);
        }
    }
}
