using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public abstract class GameConnectionProvider : IRavenConnectionProvider
    {
        private readonly ILogger logger;
        private readonly INetworkPacketController packetHandlers;
        private readonly List<RavenNetworkConnection> connections = new List<RavenNetworkConnection>();
        private readonly object mutex = new object();

        public GameConnectionProvider(ILogger logger, INetworkPacketController packetHandlers)
        {
            this.logger = logger;
            this.packetHandlers = packetHandlers;
        }

        public RavenNetworkConnection Get(MessageReader handshakeData, Connection connection)
        {
            var gameConnection = CreateConnection(logger, connection, packetHandlers);
            gameConnection.Disconnected += Connection_Disconnected;
            lock (mutex) connections.Add(gameConnection);
            return gameConnection;
        }

        public IReadOnlyList<RavenNetworkConnection> GetAll()
        {
            lock (mutex) return connections.ToList();
        }

        public IReadOnlyList<RavenNetworkConnection> GetConnected()
        {
            lock (mutex) return connections.Where(x => x.State == ConnectionState.Connected).ToList();
        }

        public T GetConnection<T>(Func<T, bool> p) where T : RavenNetworkConnection
        {
            lock (mutex) return connections.OfType<T>().FirstOrDefault(p);
        }

        public void Terminate<T, TPacket>(T activeConnection, TPacket reason) where T : RavenNetworkConnection
        {
            lock (mutex)
            {
                if (reason != null)
                {
                    activeConnection.Send(reason, SendOption.None);
                }

                activeConnection.Disconnect();
            }
        }

        protected abstract RavenNetworkConnection CreateConnection(ILogger logger, Connection connection, INetworkPacketController packetHandlers);

        private void Connection_Disconnected(object sender, System.EventArgs e)
        {
            var connection = sender as GameClientConnection;
            connection.Disconnected -= Connection_Disconnected;
            lock (mutex) connections.Remove(connection);
        }
    }

}
