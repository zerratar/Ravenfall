using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerAnimationStateUpdateHandler : INetworkPacketHandler<PlayerAnimationStateUpdate>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerAnimationStateUpdateHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerAnimationStateUpdate data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.SetAnimationState(data.PlayerId, data.AnimationState, data.Enabled, data.Trigger, data.ActionNumber);
        }
    }
}
