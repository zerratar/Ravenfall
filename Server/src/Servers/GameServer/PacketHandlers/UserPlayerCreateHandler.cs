using GameServer.Managers;
using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;

namespace GameServer.PacketHandlers
{
    public class UserPlayerCreateHandler : PlayerPacketHandler<UserPlayerCreate>
    {
        private readonly IPlayerProvider playerProvider;

        public UserPlayerCreateHandler(
            IPlayerProvider playerProvider)
        {
            this.playerProvider = playerProvider;
        }
        protected override void Handle(UserPlayerCreate data, PlayerConnection connection)
        {
            playerProvider.Create(connection.User, data.Name, data.Appearance);
            SendPlayerList(connection);
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            connection.Send(UserPlayerList.Create(playerProvider.GetPlayers(connection.User).ToArray()), SendOption.Reliable);
        }
    }
}
