using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Udp;
using System;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet
{
    public class RavenNetworkClient : IRavenNetworkConnection, IDisposable
    {
        private readonly ILogger logger;
        private readonly INetworkPacketController controller;
        private UdpClientConnection connection;

        private bool disposed;

        public Guid InstanceID => connection.InstanceID;
        public bool IsConnected => connection?.State == ConnectionState.Connected;
        public bool IsConnecting => connection?.State == ConnectionState.Connecting;

        public event EventHandler Disconnected;

        public RavenNetworkClient(
            ILogger logger,
            INetworkPacketController controller)
        {
            this.logger = logger;
            this.controller = controller;
        }

        public void ConnectAsync(IPAddress address, int port)
        {
            CreateConnection(address, port);
            connection.ConnectAsync();
        }

        public bool Connect(IPAddress address, int port)
        {
            CreateConnection(address, port);
            connection.Connect();
            return IsConnected;
        }

        private void CreateConnection(IPAddress address, int port)
        {
            if (connection != null)
            {
                connection.DataReceived -= Connection_DataReceived;
                connection.Disconnected -= Connection_Disconnected;
            }

            connection = new UdpClientConnection(new System.Net.IPEndPoint(address, port));
            connection.DataReceived += Connection_DataReceived;
            connection.Disconnected += Connection_Disconnected;
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            logger.Debug("Client disconnected.");
            Disconnected?.Invoke(this, EventArgs.Empty);
            Dispose();
        }

        public void Send<T>(short packetId, T packet, SendOption sendOption)
        {
            controller.Send(connection, packetId, packet, sendOption);
        }

        public void Send<T>(T packet, SendOption sendOption)
        {
            controller.Send(connection, packet, sendOption);
        }

        private void Connection_DataReceived(DataReceivedEventArgs obj)
        {
            controller.HandlePacketData(this, obj.Message, obj.SendOption);
        }

        public void Dispose()
        {
            if (this.disposed) return;
            if (this.connection != null)
            {
                this.connection.DataReceived -= Connection_DataReceived;
                this.connection.Disconnected -= Connection_Disconnected;
                this.connection.Dispose();
            }
            this.disposed = true;
        }
    }
}
