using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
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
}
