using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.ZoneServer.PacketHandler
{
    public class PlayerZoneEnterHandler : INetworkPacketHandler<PlayerZoneEnter>
    {
        private readonly IZone zone;

        public PlayerZoneEnterHandler(IZone zone)
        {
            this.zone = zone;
        }

        public void Handle(PlayerZoneEnter data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var response = zone.OnPlayerEnter(data.Player);
            if (response != null)
                connection.Send(response, SendOption.Reliable);
        }
    }
}
