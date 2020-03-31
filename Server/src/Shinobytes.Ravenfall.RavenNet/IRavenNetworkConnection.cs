using System;

namespace Shinobytes.Ravenfall.RavenNet
{
    public interface IRavenNetworkConnection
    {
        Guid InstanceID { get; }
        event EventHandler Disconnected;
        void Send<T>(short id, T packet, SendOption sendOption);
        void Send<T>(T packet, SendOption sendOption);
    }
}