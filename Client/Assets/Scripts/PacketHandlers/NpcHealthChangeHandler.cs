using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcHealthChangeHandler : INetworkPacketHandler<NpcHealthChange>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcHealthChangeHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcHealthChange data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var npcHandler = moduleManager.GetModule<NpcHandler>();
            npcHandler.UpdateHealth(data.NpcServerId, data.Health, data.MaxHealth, data.Delta);
        }
    }
}
