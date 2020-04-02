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
        private bool connecting;
        private UdpClientConnection connection;
        private bool disposed;

        public Guid InstanceID => connection.InstanceID;

        public event EventHandler Disconnected;

        public RavenNetworkClient(
            ILogger logger,
            INetworkPacketController controller)
        {
            this.logger = logger;
            this.controller = controller;
        }

        public void Connect(IPAddress address, int port)
        {
            if (connecting) return;
            this.connecting = true;
            this.connection = new UdpClientConnection(new System.Net.IPEndPoint(address, port));
            this.connection.DataReceived += Connection_DataReceived;
            this.connection.Disconnected += Connection_Disconnected;
            this.connection.Connect();
        }

        public void Disconnect()
        {
            this.connecting = true;
            this.connection.Disconnect("");
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            logger.Debug("Client disconnected.");
            this.Disconnected?.Invoke(this, EventArgs.Empty);
            this.Dispose();
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
            this.connection.DataReceived -= Connection_DataReceived;
            this.connection.Disconnected -= Connection_Disconnected;
            this.connection.Dispose();
            this.disposed = true;
        }
    }
}
