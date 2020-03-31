using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.ZoneServer.PacketHandler
{
    public class PlayerZoneExitHandler : INetworkPacketHandler<PlayerZoneExit>
    {
        private readonly IZone zone;

        public PlayerZoneExitHandler(IZone zone)
        {
            this.zone = zone;
        }
        public void Handle(PlayerZoneExit data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var response = this.zone.OnPlayerExit(data.PlayerId);
            if (response != null)
                connection.Send(response, SendOption.Reliable);
        }
    }
}
