using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class ZoneServerStatsHandler : INetworkPacketHandler<ServerStats>
    {
        public void Handle(ServerStats data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            if (connection is ZoneServerClientConnection zs)
            {
                zs.OnServerStats(data);
            }
        }
    }
}
