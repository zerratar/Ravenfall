using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Shinobytes.Ravenfall.HeaderlessClient.PacketHandlers
{
    public class AuthResponseHandler : INetworkPacketHandler<AuthResponse>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public AuthResponseHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(AuthResponse data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.Debug("Login response: " + data.Status);
            var auth = moduleManager.GetModule<Authentication>();
            if (auth != null)
            {
                auth.SetResult(data.Status);
            }
        }
    }

    public class MyPlayerAddHandler : INetworkPacketHandler<MyPlayerAdd>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public MyPlayerAddHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(MyPlayerAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {            
            //UnityEngine.Debug.Log("Player: " + data.Name + ", received from server. POS: " + data.Position);

            //var playerHandler = moduleManager.GetModule<PlayerHandler>();
            //var player = new Player()
            //{
            //    Id = data.PlayerId,
            //    IsMe = true,
            //    Name = data.Name,
            //    Position = data.Position,
            //};

            //playerHandler.Add(player);
            //playerHandler.PlayerStatsUpdate(player.Id, data.Experience, data.EffectiveLevel);
        }
    }
}
