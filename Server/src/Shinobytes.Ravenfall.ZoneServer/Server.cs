using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using Shinobytes.Ravenfall.ZoneServer.PacketHandler;
using System.Net;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.ZoneServer
{

    internal class Server : IRavenServer
    {
        const int gameServerPort = 8003;

        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly IZone zone;
        private readonly IProcessStatusProvider statusProvider;
        private readonly RavenNetworkClient ms;

        public Server(ILogger logger, IKernel kernel, IZone zone, IProcessStatusProvider statusProvider, INetworkPacketController msNetworkPacketController)
        {
            this.logger = logger;
            this.kernel = kernel;
            this.zone = zone;
            this.statusProvider = statusProvider;
            logger.Write("@whi@ZoneServer ");

            ms = new RavenNetworkClient(logger, GetClientPacketHandlers(msNetworkPacketController));
            kernel.SetTimeout(ReportStatus, 5000);
        }

        private void ReportStatus()
        {
            var status = statusProvider.Get();
            ms.Send(new ServerStats
            {
                CpuUsage = status.CpuUsage,
                MemUsage = status.MemUsage,
                NpcCount = zone.NpcCount,
                PlayerCount = zone.PlayerCount
            }, SendOption.None);

            kernel.SetTimeout(ReportStatus, 5000);
        }

        public IRavenServer Start()
        {
            logger.WriteLine("@mag@started");

            ms.Connect(IPAddress.Loopback, gameServerPort);
            ms.Send(new ServerHello
            {
                Name = "ZoneServer",
                ServerPort = gameServerPort,
                ServerIp = IPAddress.Loopback.ToString()
            }, SendOption.Reliable);

            logger.Debug("Connected to Game Server");

            return this;
        }

        public void Dispose()
        {
            ms.Dispose();
            kernel.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetClientPacketHandlers(INetworkPacketController controller)
        {
            return controller
                .Register<PlayerZoneEnter, PlayerZoneEnterHandler>()
                .Register<PlayerZoneExit, PlayerZoneExitHandler>()
                .Register<ServerHello>(ServerHello.OpCode)
                .Register<ServerStats>(ServerStats.OpCode);
        }
    }
}
