using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class GameClientConnection : RavenNetworkConnection
    {
        public GameClientConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
            Logger.Debug("Client[" + this.InstanceID + "] connected.");
        }

        protected override void OnDisconnected()
        {
            Logger.Debug("Client[" + this.InstanceID + "] disconnected.");
        }
    }
}
