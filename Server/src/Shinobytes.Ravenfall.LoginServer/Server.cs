using Shinobytes.Ravenfall.LoginServer.PacketHandlers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.LoginServer
{
    internal class Server : IRavenServer
    {
        const int masterServerPort = 8001;
        const int loginServerPort = 8002;

        private readonly ILogger logger;
        private readonly RavenNetworkClient ms;
        private readonly RavenNetworkServer ls;

        public Server(ILogger logger, INetworkPacketController lsNetworkPacketController, INetworkPacketController msNetworkPacketController)
        {
            this.logger = logger;

            logger.Write("@whi@LoginServer ");

            ls = new RavenNetworkServer(logger, new ServerConnectionProvider(logger, null, GetServerPacketHandlers(lsNetworkPacketController)));
            ms = new RavenNetworkClient(logger, GetClientPacketHandlers(msNetworkPacketController));
        }

        public IRavenServer Start()
        {
            ls.Start(IPAddress.Loopback, loginServerPort);
            logger.WriteLine("@mag@started @gray@on port @gre@" + loginServerPort);

            ms.Connect(IPAddress.Loopback, masterServerPort);
            ms.Send(new ServerHello
            {
                Name = "LoginServer",
                ServerPort = loginServerPort,
                ServerIp = IPAddress.Loopback.ToString()
            }, SendOption.Reliable);

            logger.Debug("Connected to MasterServer");

            return this;
        }

        public void Dispose()
        {
            ms.Dispose();
            ms.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetServerPacketHandlers(INetworkPacketController controller)
        {
            return controller.Register<AuthChallengeResponse>().Register<AuthChallenge, AuthChallengeHandler>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetClientPacketHandlers(INetworkPacketController controller)
        {
            return controller
                .Register<ServerHello>(); // ServerHello.OpCode
                                          //.Register<Dummy>();        
        }
    }
}
