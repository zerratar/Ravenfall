using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class PlayerPositionUpdateHandler : PlayerPacketHandler<PlayerPositionUpdate>
    {
        protected override void Handle(PlayerPositionUpdate data, PlayerConnection connection)
        {
            connection.Player.Position = data.Position;
        }
    }
}
