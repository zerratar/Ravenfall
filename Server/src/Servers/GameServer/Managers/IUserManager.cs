using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer.Managers
{
    public interface IUserManager
    {
        User Get(string username);
    }
}
