using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GameServer.Network
{
    public class PlayerConnectionProvider : GameConnectionProvider, IPlayerConnectionProvider
    {
        private readonly IGameSessionManager sessionManager;

        public PlayerConnectionProvider(
            ILogger logger,
            IGameSessionManager sessionManager,
            INetworkPacketController packetHandlers)
            : base(logger, packetHandlers)
        {
            this.sessionManager = sessionManager;
        }

        protected override RavenNetworkConnection CreateConnection(
            ILogger logger,
            Connection connection,
            INetworkPacketController packetController)
        {
            return new PlayerConnection(logger, connection, packetController);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PlayerConnection GetPlayerConnection(Player player)
        {
            var session = sessionManager.Get(player);

            // try and get the player connection, if its null get the session host instead.
            // as it might be a player without a client (instantiated by Twitch commands)
            return GetConnection<PlayerConnection>(x =>
                   x != null &&
                   x.Player.Id == player.Id) ?? session.Host;
        }

        /// <summary>
        /// Gets all active players regardless of their connection state.
        /// </summary>
        /// <remarks>Use this when you need to access all players regardless of their current state of connection. 
        /// This can be helpful in case some players get a temporary disconnection due to server lag.</remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<PlayerConnection> GetAllActivePlayerConnections(IGameSession session)
        {
            return GetAll()
                .OfType<PlayerConnection>()
                .Where(x => session.ContainsPlayer(x.Player));
        }

        /// <summary>
        /// Gets all active players with a known connected state
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<PlayerConnection> GetConnectedActivePlayerConnections(IGameSession session)
        {
            return GetConnected()
                .OfType<PlayerConnection>()
                .Where(x => session.ContainsPlayer(x.Player));
        }

    }
}
