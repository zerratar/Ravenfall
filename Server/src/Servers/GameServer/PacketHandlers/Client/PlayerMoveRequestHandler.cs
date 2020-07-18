using GameServer.Managers;
using GameServer.Network;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class PlayerMoveRequestHandler : PlayerPacketHandler<PlayerMoveRequest>
    {
        private readonly ILogger logger;
        private readonly IPlayerStateProvider playerState;
        private readonly IPlayerConnectionProvider connectionProvider;
        private readonly IGameSessionManager sessionManager;

        public PlayerMoveRequestHandler(
            ILogger logger,
            IPlayerStateProvider playerState,
            IPlayerConnectionProvider connectionProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.playerState = playerState;
            this.connectionProvider = connectionProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(PlayerMoveRequest data, PlayerConnection connection)
        {
            logger.Debug($"Move Request from {connection.Player.Id} from {data.Position} to {data.Destination}");

            var player = connection.Player;
            player.Position = data.Position;
            player.Destination = data.Destination;

            var session = sessionManager.Get(player);

            // player moves, release any locked objects the player may have.
            session.Objects.ReleaseLocks(player);

            // exit combat if we are in one. This will cancel any ongoing attacks.
            playerState.ExitCombat(player);

            foreach (var playerConnection in connectionProvider.GetAllActivePlayerConnections(session))
            {
                playerConnection.Send(new PlayerMoveResponse()
                {
                    PlayerId = player.Id,
                    Destination = data.Destination,
                    Position = player.Position,
                    Running = data.Running
                }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }
    }
}
