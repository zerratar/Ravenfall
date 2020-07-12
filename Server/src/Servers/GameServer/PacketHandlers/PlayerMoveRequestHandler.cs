using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.GameServer.PacketHandlers
{
    public class PlayerMoveRequestHandler : PlayerPacketHandler<PlayerMoveRequest>
    {
        private readonly ILogger logger;
        private readonly IObjectProvider objectProvider;
        private readonly IPlayerStateProvider playerState;
        private readonly IRavenConnectionProvider connectionProvider;

        public PlayerMoveRequestHandler(
            ILogger logger,
            IObjectProvider objectProvider,
            IPlayerStateProvider playerState,
            IRavenConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.objectProvider = objectProvider;
            this.playerState = playerState;
            this.connectionProvider = connectionProvider;
        }

        protected override void Handle(PlayerMoveRequest data, PlayerConnection connection)
        {
            logger.Debug($"Move Request from {connection.Player.Id} from {data.Position} to {data.Destination}");

            var player = connection.Player;
            player.Position = data.Position;
            player.Destination = data.Destination;

            // player moves, release any locked objects the player may have.
            objectProvider.ReleaseLocks(player);

            // exit combat if we are in one. This will cancel any ongoing attacks.
            playerState.ExitCombat(player);

            foreach (var playerConnection in connectionProvider.GetAll())
            {
                playerConnection.Send(new PlayerMoveResponse()
                {
                    PlayerId = player.Id,
                    Destination = data.Destination,
                    Position = player.Position,
                    Running = data.Running
                }, RavenNet.SendOption.Reliable);
            }
        }
    }
}
