using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcRespawnHandler : INetworkPacketHandler<NpcRespawn>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcRespawnHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcRespawn data, IRavenNetworkConnection connection, SendOption sendOption)
        {            
            var npcHandler = moduleManager.GetModule<NpcHandler>();            
            npcHandler.Respawn(data.NpcServerId);
        }
    }
}
