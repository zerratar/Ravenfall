using System.Collections.Generic;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer
{
    public class ZoneServerConnectionProvider : IRavenConnectionProvider
    {
        private readonly ILogger logger;
        private readonly IZoneServerProvider zoneServerProvider;
        private readonly INetworkPacketController packetHandlers;

        public ZoneServerConnectionProvider(
            ILogger logger, 
            IZoneServerProvider zoneServerProvider, 
            INetworkPacketController packetHandlers)
        {
            this.logger = logger;
            this.zoneServerProvider = zoneServerProvider;
            this.packetHandlers = packetHandlers;
        }

        public RavenNetworkConnection Get(MessageReader handshakeData, Connection connection)
        {
            return new ZoneServerClientConnection(logger, connection, zoneServerProvider, packetHandlers);
        }

        public IReadOnlyList<RavenNetworkConnection> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
