using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using System;

namespace Shinobytes.Ravenfall.RavenNet
{
    public abstract class RavenNetworkConnection : IRavenNetworkConnection, IDisposable
    {
        private readonly INetworkPacketController controller;

        protected readonly Connection Connection;
        protected readonly ILogger Logger;

        private bool disposed;

        public RavenNetworkConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetHandler)
        {
            Logger = logger;
            this.Connection = connection;
            this.controller = packetHandler;
            this.Connection.Disconnected += Connection_Disconnected;
            this.Connection.DataReceived += Connection_DataReceived;
        }

        public event EventHandler Disconnected;
        public Guid InstanceID => Connection.InstanceID;

        public object UserTag { get; set; }
        public object Tag { get; set; }
        public string SessionKey { get; set; }
        public ConnectionState State => Connection.State;
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
            controller.Send(Connection, packetId, packet, sendOption);
        }

        public void Send<TPacket>(TPacket packet, SendOption sendOption)
        {
            controller.Send(Connection, packet, sendOption);
        }

        public void Disconnect()
        {
            Connection.Disconnect(null);
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            OnDisconnected();
            Disconnected?.Invoke(this, e);
            Connection.Disconnected -= Connection_Disconnected;
            Connection.DataReceived -= Connection_DataReceived;
            this.Dispose();
        }

        protected abstract void OnDisconnected();

        public void Dispose()
        {
            if (this.disposed) return;
            this.Connection.Dispose();
            this.disposed = true;
        }
    }
}
