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
    public class BotAuthRequestHandler : BotPacketHandler<BotAuthRequest>
    {
        private readonly ILogger logger;
        private readonly IUserManager userProvider;
        private readonly IAuthService authService;
        private readonly IStreamBotFactory botProvider;
        private readonly IStreamBotManager botManager;

        public BotAuthRequestHandler(
            ILogger logger,
            IUserManager userProvider,
            IAuthService authService,
            IStreamBotFactory botProvider,
            IStreamBotManager botManager)
        {
            this.logger = logger;
            this.userProvider = userProvider;
            this.authService = authService;
            this.botProvider = botProvider;
            this.botManager = botManager;
        }

        protected override void Handle(BotAuthRequest data, BotConnection connection)
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

            var bot = botProvider.Create(connection);
            botManager.Add(bot);

            connection.UserTag = user;
            connection.Tag = bot;
            connection.Disconnected -= ClientDisconnected;
            connection.Disconnected += ClientDisconnected;

            // authenticated
            // send auth response
            SendSuccessLoginResult(connection);
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            var connection = sender as BotConnection;
            connection.Disconnected -= ClientDisconnected;
            botManager.Remove(connection.Bot);
        }

        private void SendFailedLoginResult(BotConnection connection, AuthResult result)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = (int)result,
                SessionKeys = new byte[0]
            }, SendOption.Reliable);
        }
        private void SendSuccessLoginResult(BotConnection connection)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = 0,
                SessionKeys = new byte[4] { 1, 2, 3, 4 }
            }, SendOption.Reliable);
        }
    }
}
