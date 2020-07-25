using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class NpcAddHandler : INetworkPacketHandler<NpcAdd>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public NpcAddHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(NpcAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("Add Instance Id: " + data.NpcId + ", Npc Id: " + data.ServerId + " received from server.");
            var npcHandler = moduleManager.GetModule<NpcHandler>();
            var npc = new Npc()
            {
                NpcId = data.NpcId,
                Id = data.ServerId,
                Position = data.Position,
                Destination = data.Destination,
                Rotation = data.Rotation,
                Attributes = data.Attributes,
                Endurance = data.Endurance,
                Health = data.Health,
                Level = data.Level
            };

            npcHandler.Add(npc);
        }
    }
}
