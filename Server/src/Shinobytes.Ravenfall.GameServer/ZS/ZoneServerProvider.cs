using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Shinobytes.Ravenfall.GameServer
{
    public class ZoneServerProvider : IZoneServerProvider
    {
        private readonly ConcurrentDictionary<Guid, IRavenNetworkConnection> zoneServers = new ConcurrentDictionary<Guid, IRavenNetworkConnection>();
        private readonly ILogger logger;
        private int count;

        public ZoneServerProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public int Register(IRavenNetworkConnection connection)
        {
            connection.Disconnected += Connection_Disconnected;
            zoneServers[connection.InstanceID] = connection;
            return Interlocked.Increment(ref count);
        }

        /// <summary>
        ///     Gets the first best available Zone Server connection. It will try and get the least used server first.
        /// </summary>
        /// <returns></returns>
        public IRavenNetworkConnection Get()
        {
            if (zoneServers.Count == 0) return null;
            if (zoneServers.Count == 1) return zoneServers.Values.FirstOrDefault();
            var servers = zoneServers.Values.Cast<ZoneServerClientConnection>().ToList();
            return servers.OrderBy(x => x.Stats.CpuUsage + x.Stats.MemUsage).FirstOrDefault();
        }

        private void Connection_Disconnected(object sender, EventArgs e)
        {
            var connection = sender as IRavenNetworkConnection;
            connection.Disconnected -= Connection_Disconnected;
            zoneServers.TryRemove(connection.InstanceID, out _);
            logger.Debug("Zone Server removed.");
        }
    }
}
