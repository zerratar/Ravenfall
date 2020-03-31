using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.FrontServer.PacketHandlers
{
    public class ServerHelloHandler : INetworkPacketHandler<ServerHello>
    {
        public void Handle(ServerHello data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var client = connection as ServerClientConnection;
            if (client == null) return;
            if (data == null) return;
            client.OnServerDiscovery(data.Name, data.ServerIp, data.ServerPort);
        }
    }
}
