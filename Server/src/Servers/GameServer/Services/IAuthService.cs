using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace GameServer.Services
{
    public interface IAuthService
    {
        AuthResult Authenticate(User user, string password);        
    }
}
