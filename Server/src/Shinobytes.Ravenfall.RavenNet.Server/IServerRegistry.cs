using System.Collections.Concurrent;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IServerRegistry
    {
        void Register(string name, ServerClientConnection server);
        ServerClientConnection GetServerConnection(string name);
    }

    public class ServerRegistry : IServerRegistry
    {
        private readonly ConcurrentDictionary<string, ServerClientConnection> servers = new ConcurrentDictionary<string, ServerClientConnection>();

        public ServerClientConnection GetServerConnection(string name)
        {
            if (servers.TryGetValue(name, out var server))
                return server;

            throw new System.Exception("Server with name '" + name + "' could not be found.");
        }

        public void Register(string name, ServerClientConnection server)
        {
            servers[name] = server;
        }
    }
}
