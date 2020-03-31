using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.FrontServer.PacketHandlers
{
    public class DummyHandler : INetworkPacketHandler<Dummy>
    {
        private int callCount;
        public void Handle(Dummy data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var client = connection as ServerClientConnection;
            if (client == null) return;
            if (data == null) return;
            ++callCount;
        }
    }
}
