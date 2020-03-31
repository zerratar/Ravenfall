using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class ObjectUpdateHandler : INetworkPacketHandler<ObjectUpdate>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public ObjectUpdateHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(ObjectUpdate data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("Update Instance Id: " + data.ObjectServerId + ", Object Id: " + data.ObjectId + " received from server.");
            var objectHandler = moduleManager.GetModule<ObjectHandler>();
            var obj = new SceneObject()
            {
                Id = data.ObjectServerId,
                ObjectId = data.ObjectId,
                Position = data.Position,
            };

            objectHandler.Update(obj);
        }
    }
}
