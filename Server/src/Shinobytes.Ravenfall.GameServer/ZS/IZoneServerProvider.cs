using Shinobytes.Ravenfall.RavenNet;

namespace Shinobytes.Ravenfall.GameServer
{
    public interface IZoneServerProvider
    {
        int Register(IRavenNetworkConnection connection);
        IRavenNetworkConnection Get();
    }
}
