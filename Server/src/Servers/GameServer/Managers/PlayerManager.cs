using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public class PlayerManager : IPlayerManager
    {

        private readonly List<Player> activePlayers = new List<Player>();
        private readonly object mutex = new object();

        public IReadOnlyList<Player> GetAll()
        {
            lock (mutex)
            {
                return activePlayers;
            }
        }
        public void Add(Player player)
        {
            lock (mutex)
            {
                activePlayers.Add(player);
            }
        }

        public void Remove(Player player)
        {
            lock (mutex)
            {
                activePlayers.Remove(player);
            }
        }
    }
}
