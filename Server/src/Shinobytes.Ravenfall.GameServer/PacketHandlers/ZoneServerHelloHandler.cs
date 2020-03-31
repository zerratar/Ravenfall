using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class ZoneServerHelloHandler : INetworkPacketHandler<ServerHello>
    {
        private readonly ILogger logger;
        public ZoneServerHelloHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public void Handle(ServerHello data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            if (connection is ZoneServerClientConnection zs)
            {
                zs.OnServerDiscovery(data.Name);
            }
        }
    }
}
