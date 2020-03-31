using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class AuthRequestHandler : PlayerPacketHandler<AuthRequest>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IRavenConnectionProvider connectionProvider;
        private readonly IPlayerProvider playerProvider;

        public AuthRequestHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IRavenConnectionProvider connectionProvider,
            IPlayerProvider playerProvider)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.connectionProvider = connectionProvider;
            this.playerProvider = playerProvider;
        }

        protected override void Handle(AuthRequest data, PlayerConnection connection)
        {
            logger.Debug("Auth Request received. User: " + data.Username + ", Pass: " + data.Password + ", ClientVersion: " + data.ClientVersion);
            logger.Debug("Sending Auth Response: " + 0);

            connection.Disconnected += ClientConnection_Disconnected;
            connection.Tag = playerProvider.Get(data.Username);
            connection.Send(new AuthResponse() { Status = 0, SessionKeys = new byte[4] { 1, 2, 3, 4 } }, SendOption.Reliable);

            worldProcessor.AddPlayer(connection);
        }

        private void ClientConnection_Disconnected(object sender, System.EventArgs e)
        {
            var connection = sender as PlayerConnection;
            connection.Disconnected -= ClientConnection_Disconnected;

            if (playerProvider.Remove(connection.Player.Id))
                worldProcessor.RemovePlayer(connection.Player);
        }
    }
}
