using Shinobytes.Ravenfall.HeaderlessClient.PacketHandlers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using System.Net;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.TestClient
{
    public class Client : IRavenClient
    {
        private readonly ILogger logger;
        private readonly RavenNetworkClient client;
        private readonly Authentication auth;

        public IModuleManager Modules { get; }

        public Client(ILogger logger, IModuleManager moduleManager, INetworkPacketController controller)
        {
            this.logger = logger;
            this.Modules = moduleManager;

            this.client = new RavenNetworkClient(logger, GetPacketController(controller));
            this.auth = this.Modules.AddModule(new Authentication(this.client));
        }

        public void Connect(IPAddress address, int port)
        {
            client.Connect(address, port);
            logger.Debug("Connected to FS");

            //var count = 500_000;
            //var sw = new Stopwatch();
            //sw.Start();
            //for (var i = 0; i < count; ++i)
            //{
            //    client.Send(new Dummy() { }, SendOption.None);
            //}
            //sw.Stop();
            //logger.WriteLine("@yel@" + count + " packets sent in @whi@" + sw.Elapsed.TotalSeconds + "@yel@ seconds. Avg: @whi@" + (count / sw.Elapsed.TotalSeconds) + " @yel@per second");

        }

        public void Dispose()
        {
            Modules.Dispose();
            client.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INetworkPacketController GetPacketController(INetworkPacketController controller)
        {
            return controller.Register<AuthResponse, AuthResponseHandler>();
        }
    }
}
