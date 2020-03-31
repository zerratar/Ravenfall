using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Net;

namespace Shinobytes.Ravenfall.FrontServer
{
    internal class Server : IRavenServer
    {
        const int gameServerPort = 8000;

        private readonly ILogger logger;
        private readonly RavenNetworkServer server;

        public Server(
            ILogger logger,
            IRavenConnectionProvider connectionProvider)
        {
            this.logger = logger;
            logger.Write("@whi@Server ");
            server = new RavenNetworkServer(logger, connectionProvider);
        }

        public IRavenServer Start()
        {
            server.Start(IPAddress.Any, gameServerPort);
            logger.WriteLine("@mag@started @gray@on port @gre@" + gameServerPort);
            return this;
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
