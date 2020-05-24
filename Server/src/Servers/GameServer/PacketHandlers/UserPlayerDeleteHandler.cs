using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class UserPlayerDeleteHandler : PlayerPacketHandler<UserPlayerDelete>
    {
        private readonly IPlayerProvider playerProvider;

        public UserPlayerDeleteHandler(
            IPlayerProvider playerProvider)
        {
            this.playerProvider = playerProvider;
        }

        protected override void Handle(UserPlayerDelete data, PlayerConnection connection)
        {
            var player = playerProvider.Get(data.PlayerId);
            if (player == null)
            {
                return;
            }

            playerProvider.Remove(player.Id);
            SendPlayerList(connection);
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            connection.Send(UserPlayerList.Create(playerProvider.GetPlayers(connection.User).ToArray()), SendOption.Reliable);
        }
    }
}
