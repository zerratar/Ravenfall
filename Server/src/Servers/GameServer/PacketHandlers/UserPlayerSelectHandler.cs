using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class UserPlayerSelectHandler : PlayerPacketHandler<UserPlayerSelect>
    {
        private readonly IWorldProcessor worldProcessor;
        private readonly IPlayerProvider playerProvider;

        public UserPlayerSelectHandler(
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider)
        {
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
        }

        protected override void Handle(UserPlayerSelect data, PlayerConnection connection)
        {
            var player = playerProvider.Get(data.PlayerId);
            if (player == null)
            {
                return;
            }

            // the disconnected event is only interesting after a player has
            // been selected, since the server wont keep track on a logged in user
            // without a selected player.

            connection.Disconnected -= ClientDisconnected;
            connection.Disconnected += ClientDisconnected;
            connection.PlayerTag = player;
            connection.SessionKey = data.SessionKey;
            worldProcessor.AddPlayer(data.SessionKey, connection);
        }

        private void ClientDisconnected(object sender, System.EventArgs e)
        {
            var connection = sender as PlayerConnection;
            connection.Disconnected -= ClientDisconnected;

            if (connection.Player == null)
                return;

            if (playerProvider.Remove(connection.Player.Id))
                worldProcessor.RemovePlayer(connection.Player);
        }
    }
}
