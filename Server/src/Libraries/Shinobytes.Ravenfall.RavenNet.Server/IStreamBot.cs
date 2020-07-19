using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IStreamBot
    {
        int AvailabilityScore { get; }
        void Disconnect(User user);
        void Connect(User user);
    }
}
