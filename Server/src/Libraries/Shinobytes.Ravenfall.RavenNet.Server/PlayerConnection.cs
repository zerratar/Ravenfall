using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class PlayerConnection : UserConnection
    {
        public PlayerConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
        }

        public Player Player => Tag as Player;
    }

}
