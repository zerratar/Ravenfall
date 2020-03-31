using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerAddHandler : INetworkPacketHandler<PlayerAdd>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerAddHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.Log("Player: " + data.Name + ", received from server.");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            var player = new Player()
            {
                Id = data.PlayerId,
                IsMe = false,
                Name = data.Name,
                Position = data.Position,
                Destination = data.Destination
            };

            playerHandler.Add(player);
        }
    }
}
