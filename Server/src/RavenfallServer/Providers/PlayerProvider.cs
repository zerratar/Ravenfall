
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RavenfallServer.Providers
{
    public class PlayerProvider : IPlayerProvider
    {
        private List<Player> players = new List<Player>();
        private readonly object mutex = new object();
        private int userIndex = 0;

        public Player Get(string username)
        {
            return GetOrAddPlayer(username);
        }

        public Player Get(int playerId)
        {
            lock (mutex) return players.FirstOrDefault(x => x.Id == playerId);
        }

        public bool Remove(int playerId)
        {
            lock (mutex)
            {
                var player = Get(playerId);
                if (player == null) return false;
                return players.Remove(player);
            }
        }

        private Player GetOrAddPlayer(string username)
        {
            lock (mutex)
            {
                var player = players.FirstOrDefault(x => x.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (player != null) return player;

                var id = Interlocked.Increment(ref userIndex);
                var random = new Random();
                var pos = new Vector3((float)random.NextDouble() * 2f, 0, (float)random.NextDouble() * 2f);
                var addedPlayer = new Player()
                {
                    Id = id,
                    Name = username,
                    Position = pos,
                    Destination = pos,
                };

                players.Add(addedPlayer);
                return addedPlayer;
            }
        }

        public IReadOnlyList<Player> GetAll()
        {
            lock (mutex) return players.ToList();
        }
    }
}
