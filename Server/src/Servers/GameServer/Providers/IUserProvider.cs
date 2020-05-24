using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface IUserProvider
    {
        User Get(string username);
    }
}
