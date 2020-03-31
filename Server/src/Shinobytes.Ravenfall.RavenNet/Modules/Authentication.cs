using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using System;
using System.Threading;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class Authentication : IModule
    {
        private const int LOGIN_SUCCESS = 0;
        private const int LOGIN_INVALID = 1;
        private const int LOGIN_BANNED = 2;

        private readonly IRavenNetworkConnection connection;
        private int activeAuthRequest;

        public event EventHandler LoginSuccess;
        public event EventHandler<LoginFailedEventArgs> LoginFailed;

        public Authentication(IRavenNetworkConnection connection)
        {
            this.connection = connection;
        }

        public string Name => "Auth";
        public bool Authenticated { get; set; }
        public bool Authenticating => Volatile.Read(ref activeAuthRequest) > 0;

        public void Authenticate(string username, string password)
        {
            if (Interlocked.CompareExchange(ref activeAuthRequest, 1, 0) == 1)
            {
                return;
            }

            this.connection.Send(new AuthRequest()
            {
                Username = username,
                Password = password
            }, SendOption.Reliable);
        }

        public void SetResult(int status)
        {
            Authenticated = status == 0;
            try
            {
                switch (status)
                {
                    case LOGIN_SUCCESS:
                        LoginSuccess?.Invoke(this, EventArgs.Empty);
                        return;
                    case LOGIN_INVALID:
                        LoginFailed?.Invoke(this, new LoginFailedEventArgs(false));
                        return;
                    case LOGIN_BANNED:
                        LoginFailed?.Invoke(this, new LoginFailedEventArgs(true));
                        return;
                }
            }
            finally
            {
                Interlocked.CompareExchange(ref activeAuthRequest, 0, 1);
            }
        }

        public class LoginFailedEventArgs : EventArgs
        {
            public LoginFailedEventArgs(bool isBanned)
            {
                IsBanned = isBanned;
            }

            public bool IsBanned { get; }
        }
    }
}
