using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Udp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class RavenNetworkServer : IRavenNetworkServer, IDisposable
    {
        private UdpConnectionListener server;
        private readonly ILogger logger;
        private readonly IRavenConnectionProvider connectionProvider;
        private readonly List<RavenNetworkConnection> connections = new List<RavenNetworkConnection>();
        private readonly object mutex = new object();
        private bool disposed;

        public RavenNetworkServer(ILogger logger, IRavenConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.connectionProvider = connectionProvider;
        }

        public void Start(IPAddress address, int port)
        {
            this.server = new UdpConnectionListener(new IPEndPoint(address, port), IPMode.IPv4, msg => logger.WriteLine(msg));
            this.server.NewConnection += Server_NewConnection;
            this.server.Start();
        }

        private void Server_NewConnection(NewConnectionEventArgs obj)
        {
            var connection = connectionProvider.Get(obj.HandshakeData, obj.Connection);
            if (connection == null) return;
            connection.Disconnected += Connection_Disconnected;
            lock (mutex)
            {
                this.connections.Add(connection);
            }
        }

        private void Connection_Disconnected(object sender, EventArgs e)
        {
            var connection = sender as RavenNetworkConnection;
            lock (mutex)
            {
                this.connections.Remove(connection);
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            this.server.Close();
            this.disposed = true;
        }
    }

}
