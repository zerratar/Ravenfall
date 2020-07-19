using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class BotConnection : UserConnection
    {
        public BotConnection(
          ILogger logger,
          Connection connection,
          INetworkPacketController packetHandler)
          : base(logger, connection, packetHandler)
        {
        }

        public IStreamBot Bot => Tag as IStreamBot;
    }

}
