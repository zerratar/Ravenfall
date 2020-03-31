using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace RavenfallServer.Network
{
    public class PlayerConnectionProvider : GameConnectionProvider
    {
        public PlayerConnectionProvider(
            ILogger logger,
            INetworkPacketController packetHandlers)
            : base(logger, packetHandlers)
        {
        }

        protected override RavenNetworkConnection CreateConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetController)
        {
            return new PlayerConnection(logger, connection, packetController);
        }
    }
}
