using System;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class ServerClientConnection : RavenNetworkConnection
    {
        private readonly IServerRegistry registry;

        public ServerClientConnection(
            ILogger logger,
            Connection connection,
            IServerRegistry registry,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
            this.registry = registry;
        }

        public string Name { get; private set; }
        public string ServerIp { get; private set; }
        public int ServerPort { get; private set; }

        protected override void OnDisconnected()
        {
            Logger.Debug($"[@whi@{this.Name ?? "???"}@gray@] Connection Closed.");
        }

        public void OnServerDiscovery(string name, string serverIp, int serverPort)
        {
            registry.Register(name, this);

            this.Name = name;
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
            Logger.Debug($"[@whi@{this.Name ?? "???"}@gray@] Server Discovery. {serverIp}:{serverPort}");
        }
    }
}
