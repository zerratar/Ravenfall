using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.GameServer
{
    public class ZoneServerClientConnection : RavenNetworkConnection
    {
        private readonly IZoneServerProvider zoneServers;
        private int index;

        public ZoneServerClientConnection(
            ILogger logger,
            Connection connection,
            IZoneServerProvider zoneServers,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
            this.zoneServers = zoneServers;
        }

        public string Name { get; private set; }
        public ServerStats Stats { get; private set; }

        protected override void OnDisconnected()
        {
            Logger.Debug($"[@whi@ZoneServer@yel@#{index}@gray@] Connection Closed.");
        }

        public void OnServerDiscovery(string name)
        {
            index = zoneServers.Register(this);
            Name = name;
            Logger.Debug($"[@whi@{this.Name ?? "???"}@yel@#{index}@gray@] Server Discovery.");
        }

        internal void OnServerStats(ServerStats data)
        {
            Logger.Debug($"[@whi@{this.Name ?? "???"}@yel@#{index}@gray@] Stats - CPU: @yel@" + data.CpuUsage + "%@gray@ MEM: @yel@" + data.MemUsage + "MB@gray@ Players: @yel@" + data.PlayerCount + " @gray@NPCs: @yel@" + data.NpcCount);
            this.Stats = data;
        }
    }
}
