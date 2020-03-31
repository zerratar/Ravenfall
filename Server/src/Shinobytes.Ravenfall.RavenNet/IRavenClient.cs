using Shinobytes.Ravenfall.RavenNet.Modules;
using System;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet
{

    public interface IRavenClient : IDisposable
    {
        void Connect(IPAddress address, int port);
        IModuleManager Modules { get; }
    }
}
