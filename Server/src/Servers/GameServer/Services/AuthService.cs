using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace GameServer.Services
{
    public class AuthService : IAuthService
    {
        public AuthResult Authenticate(User user, string password)
        {
            if (user == null) return AuthResult.InvalidPassword;
            return AuthResult.Success;
        }
    }
}
