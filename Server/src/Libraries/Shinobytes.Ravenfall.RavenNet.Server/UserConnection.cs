using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class UserConnection : GameClientConnection
    {
        public UserConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
        }


        public User User => UserTag as User;
    }
}
