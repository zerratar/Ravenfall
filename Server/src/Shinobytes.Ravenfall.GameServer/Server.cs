using Shinobytes.Ravenfall.GameServer.PacketHandlers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System.Net;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.GameServer
{
    internal class Server : IRavenServer
    {
        const int masterServerPort = 8001;
        const int gameServerPort = 8003;

        private readonly ILogger logger;
        private readonly IZoneServerProvider zoneServerProvider;
        private readonly RavenNetworkClient ms;
        private readonly RavenNetworkServer gs;

        public Server(ILogger logger, IZoneServerProvider zoneServerProvider, INetworkPacketController lsNetworkPacketController, INetworkPacketController msNetworkPacketController)
        {
            this.logger = logger;
            this.zoneServerProvider = zoneServerProvider;
            logger.Write("@whi@GameServer ");

            gs = new RavenNetworkServer(logger, new ZoneServerConnectionProvider(logger, zoneServerProvider, GetServerPacketHandlers(lsNetworkPacketController)));
            ms = new RavenNetworkClient(logger, GetClientPacketHandlers(msNetworkPacketController));
        }

        public IRavenServer Start()
        {
            gs.Start(IPAddress.Loopback, gameServerPort);
            logger.WriteLine("@mag@started @gray@on port @gre@" + gameServerPort);

            ms.Connect(IPAddress.Loopback, masterServerPort);
            ms.Send(new ServerHello
            {
                Name = "GameServer",
                ServerPort = gameServerPort,
                ServerIp = IPAddress.Loopback.ToString()
            }, SendOption.Reliable);

            logger.Debug("Connected to MasterServer");

            return this;
        }

        public void Dispose()
        {
            ms.Dispose();
            gs.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetServerPacketHandlers(INetworkPacketController controller)
        {
            return controller
                .Register<ServerHello, ZoneServerHelloHandler>()
                .Register<ServerStats, ZoneServerStatsHandler>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetClientPacketHandlers(INetworkPacketController controller)
        {
            return controller.Register<ServerHello>(ServerHello.OpCode);
        }
    }
}
