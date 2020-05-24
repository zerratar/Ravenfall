using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class ConnectionKillSwitchHandler : INetworkPacketHandler<ConnectionKillSwitch>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public ConnectionKillSwitchHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(ConnectionKillSwitch data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.Log($"Disconnected by server. Reason: {data.Reason}");
        }
    }
}
