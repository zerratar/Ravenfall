using Shinobytes.Ravenfall.RavenNet.Modules;
using System;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet
{
    public interface IRavenClient : IDisposable
    {
        bool Connect(IPAddress address, int port);
        void ConnectAsync(IPAddress address, int port);
        IModuleManager Modules { get; }
        bool IsConnecting { get; }
        bool IsConnected { get; }
        void Send<T>(short packetId, T packet, SendOption sendOption);
        void Send<T>(T packet, SendOption sendOption);
    }
}
