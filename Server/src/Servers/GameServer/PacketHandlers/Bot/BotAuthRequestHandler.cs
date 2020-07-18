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
    public class BotAuthRequestHandler : PlayerPacketHandler<BotAuthRequest>
    {
        private readonly ILogger logger;
        private readonly IUserManager userProvider;
        private readonly IAuthService authService;

        public BotAuthRequestHandler(
            ILogger logger,
            IUserManager userProvider,
            IAuthService authService)
        {
            this.logger = logger;
            this.userProvider = userProvider;
            this.authService = authService;
        }

        protected override void Handle(BotAuthRequest data, PlayerConnection connection)
        {
            logger.Debug("Bot Auth Request received. User: " + data.Username + ", Pass: " + data.Password);
            logger.Debug("Sending Auth Response: " + 0);

            var user = userProvider.Get(data.Username);
            var result = authService.Authenticate(user, data.Password);

            if (result != AuthResult.Success)
            {
                SendFailedLoginResult(connection, result);
                return;
            }            

            connection.UserTag = user;

            // authenticated
            // send auth response
            SendSuccessLoginResult(connection);
        }
        private void SendFailedLoginResult(PlayerConnection connection, AuthResult result)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = (int)result,
                SessionKeys = new byte[0]
            }, SendOption.Reliable);
        }
        private void SendSuccessLoginResult(PlayerConnection connection)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = 0,
                SessionKeys = new byte[4] { 1, 2, 3, 4 }
            }, SendOption.Reliable);
        }
    }
}
