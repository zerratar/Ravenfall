using Shinobytes.Ravenfall.FrontServer.PacketHandlers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System.Net;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.FrontServer
{
    internal class Server : IRavenServer
    {
        const int frontServerPort = 8000;
        const int masterServerPort = 8001;

        private readonly ILogger logger;
        private readonly RavenNetworkServer fs;
        private readonly RavenNetworkServer ms;

        public Server(
            ILogger logger, 
            IServerRegistry serverRegistry, 
            INetworkPacketController fsNetworkPacketController, 
            INetworkPacketController msNetworkPacketController)
        {
            this.logger = logger;

            logger.Write("@whi@FrontServer@gray@+@whi@MasterServer ");
            this.fs = new RavenNetworkServer(logger, new GameConnectionProvider(logger, GetClientPacketHandlers(fsNetworkPacketController)));
            this.ms = new RavenNetworkServer(logger, new ServerConnectionProvider(logger, serverRegistry, GetServerPacketHandlers(msNetworkPacketController)));
        }

        public IRavenServer Start()
        {
            fs.Start(IPAddress.Any, frontServerPort);
            ms.Start(IPAddress.Loopback, masterServerPort);
            logger.WriteLine("@mag@started @gray@on port @gre@" + frontServerPort + "@gray@ and @gre@" + masterServerPort);
            return this;
        }

        public void Dispose()
        {
            fs.Dispose();
            ms.Dispose();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetServerPacketHandlers(INetworkPacketController controller)
        {
            return controller
                .Register<AuthChallengeResponse>()
                .Register<ServerHello, ServerHelloHandler>() // ServerHello.OpCode
                .Register<RavenNet.Packets.Client.Dummy, DummyHandler>(); // Dummy.OpCode
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetClientPacketHandlers(INetworkPacketController controller)
        {
            return controller
                .Register<AuthRequest, AuthRequestHandler>();
        }
    }
}
