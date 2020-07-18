using GameServer.Managers;
using GameServer.Network;
using GameServer.Services;
using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;
using System.Linq;

namespace GameServer.PacketHandlers
{
    public class AuthRequestHandler : PlayerPacketHandler<AuthRequest>
    {
        private readonly ILogger logger;
        private readonly IPlayerProvider playerProvider;
        private readonly IUserManager userProvider;
        private readonly IAuthService authService;
        private readonly IPlayerConnectionProvider connectionProvider;

        public AuthRequestHandler(
            ILogger logger,
            IPlayerProvider playerProvider,
            IUserManager userProvider,
            IAuthService authService,
            IPlayerConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.playerProvider = playerProvider;
            this.userProvider = userProvider;
            this.authService = authService;
            this.connectionProvider = connectionProvider;
        }

        protected override void Handle(AuthRequest data, PlayerConnection connection)
        {
            logger.Debug("Auth Request received. User: " + data.Username + ", Pass: " + data.Password + ", ClientVersion: " + data.ClientVersion);
            logger.Debug("Sending Auth Response: " + 0);

            var user = userProvider.Get(data.Username);
            var result = authService.Authenticate(user, data.Password);

            if (result != AuthResult.Success)
            {
                connection.Send(new AuthResponse() { Status = (int)result, SessionKeys = new byte[0] }, SendOption.Reliable);
                return;
            }

            // check if we already have a connection with the same user
            // and kick that user out if so by disconnecting that connection.
            var activeConnection = connectionProvider.GetConnection<PlayerConnection>(x => x.User?.Id == user.Id);
            if (activeConnection != null)
            {
                connectionProvider.Terminate(activeConnection, ConnectionKillSwitch.MultipleLocations);
            }

            connection.UserTag = user;

            // authenticated
            // send auth response
            SendSuccessLoginResult(connection);

            // then send player list
            SendPlayerList(connection);

            // when player has been selected do the following
            //connection.PlayerTag = playerProvider.Get(data.Username);
            //worldProcessor.AddPlayer(connection);
            // SEE: UserPlayerSelectHandler, logic moved there.
        }

        private void SendSuccessLoginResult(PlayerConnection connection)
        {
            connection.Send(new AuthResponse()
            {
                Status = 0,
                SessionKeys = new byte[4] { 1, 2, 3, 4 }
            }, SendOption.Reliable);
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            connection.Send(UserPlayerList.Create(playerProvider.GetPlayers(connection.User).ToArray()), SendOption.Reliable);
        }
    }
}
