using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using System;

namespace Shinobytes.Ravenfall.RavenNet
{
    [Obsolete("Do not use")]
    public abstract class RavenNetworkConnection : IRavenNetworkConnection, IDisposable
    {
        private readonly Connection connection;
        private readonly INetworkPacketController controller;

        protected readonly ILogger Logger;
        private bool disposed;

        public event EventHandler Disconnected;

        public Guid InstanceID => connection.InstanceID;

        public RavenNetworkConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetHandler)
        {
            Logger = logger;
            this.connection = connection;
            this.controller = packetHandler;
            this.connection.Disconnected += Connection_Disconnected;
            this.connection.DataReceived += Connection_DataReceived;
        }

        private void Connection_DataReceived(DataReceivedEventArgs obj)
        {
            controller.HandlePacketData(this, obj.Message, obj.SendOption);
            //Logger.WriteLine("Packet received");
        }

        public void RequestNonBlocking<TRequest, TResponse>(TRequest request, Func<TResponse, bool> filter)
        {
            controller.AddFilter(filter);
            Send(request, SendOption.Reliable);
        }

        public void Send<TPacket>(short packetId, TPacket packet, SendOption sendOption)
        {
            controller.Send(connection, packetId, packet, sendOption);
        }

        public void Send<TPacket>(TPacket packet, SendOption sendOption)
        {
            controller.Send(connection, packet, sendOption);
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            OnDisconnected();
            Disconnected?.Invoke(this, e);
            connection.Disconnected -= Connection_Disconnected;
            connection.DataReceived -= Connection_DataReceived;
            this.Dispose();
        }

        protected abstract void OnDisconnected();

        public void Dispose()
        {
            if (this.disposed) return;
            this.connection.Dispose();
            this.disposed = true;
        }
    }
}
