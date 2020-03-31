using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.ZoneServer
{

    /// <summary>
    /// 1: what is a zone?
    ///   - A zone is a gameplay area that requires loading. This is to ensure we dont have to communicate between zones to
    ///     give players information about others outside their zones.
    /// 2: how big is a zone?
    ///   - The zones will or may vary in sizes. Such as an game instance, a city or a large piece of the map. A zone should be able to
    ///     handle a couple of hundres of players before becoming too slow. But we don't know that yet. Kappa
    /// </summary>
    public class Zone : IZone
    {
        private const float PlayerInterestDistance = 25f;

        private ConcurrentDictionary<int, Player> players = new ConcurrentDictionary<int, Player>();

        public int NpcCount => 0;

        public int PlayerCount => 0;

        public PlayerEnterUpdate OnPlayerEnter(Player player)
        {
            //  players[player.Id] = player;

            var update = new PlayerEnterUpdate();
            if (players.TryAdd(player.Id, player))
            {
                // find players close to this one. Let them know the player has arrived.
                var players = GetPlayersInInterestArea(player);
                update.PlayersToUpdate = players.Select(x => x.Id).ToArray();
                update.Data = player;
            }

            return update;
        }

        public PlayerExitUpdate OnPlayerExit(int playerId)
        {
            var update = new PlayerExitUpdate();
            if (players.TryRemove(playerId, out var player))
            {
                // find players around, remove player from their area
                // we have an interest area that is smaller than the zone
                // for each player to avoid pushing too much information
                var players = GetPlayersInInterestArea(player);
                update.PlayersToUpdate = players.Select(x => x.Id).ToArray();
                update.Data = player;

            }

            return update;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IReadOnlyList<Player> GetPlayersInInterestArea(Player player)
        {
            return this.players.Values.Where(x => x.Id != player.Id && Vector3.Distance(x.Position, player.Position) <= PlayerInterestDistance).ToList();
        }
    }
}
