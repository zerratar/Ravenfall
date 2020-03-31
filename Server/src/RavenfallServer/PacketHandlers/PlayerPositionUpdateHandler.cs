using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.FrontServer.PacketHandlers
{
    public class PlayerPositionUpdateHandler : PlayerPacketHandler<PlayerPositionUpdate>
    {
        protected override void Handle(PlayerPositionUpdate data, PlayerConnection connection)
        {
            var player = connection.Player;
            player.Position = data.Position;
        }
    }
}
