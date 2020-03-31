using System;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IRavenServer : IDisposable
    {
        IRavenServer Start();
    }
}
