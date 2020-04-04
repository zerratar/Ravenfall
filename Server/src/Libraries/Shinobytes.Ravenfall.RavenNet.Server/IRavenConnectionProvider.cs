using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IRavenConnectionProvider
    {
        RavenNetworkConnection Get(MessageReader handshakeData, Connection connection);

        IReadOnlyList<RavenNetworkConnection> GetAll();
        IReadOnlyList<RavenNetworkConnection> GetConnected();
        T GetConnection<T>(Func<T, bool> p);
    }

}
