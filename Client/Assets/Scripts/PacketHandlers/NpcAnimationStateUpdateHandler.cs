using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcAnimationStateUpdateHandler : INetworkPacketHandler<NpcAnimationStateUpdate>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcAnimationStateUpdateHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcAnimationStateUpdate data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var playerHandler = moduleManager.GetModule<NpcHandler>();
            playerHandler.SetAnimationState(data.NpcServerId, data.AnimationState, data.Enabled, data.Trigger, data.ActionNumber);
        }
    }

}
