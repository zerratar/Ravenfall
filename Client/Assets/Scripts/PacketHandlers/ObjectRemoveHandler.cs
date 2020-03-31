using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class ObjectRemoveHandler : INetworkPacketHandler<ObjectRemove>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public ObjectRemoveHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(ObjectRemove data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("Remove Instance Id: " + data.ObjectServerId + " received from server.");
            var objectHandler = moduleManager.GetModule<ObjectHandler>();
            var obj = new SceneObject()
            {
                Id = data.ObjectServerId
            };

            objectHandler.Remove(obj);
        }
    }
}
