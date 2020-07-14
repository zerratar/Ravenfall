using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IPlayerManager
    {
        void Add(Player player);
        void Remove(Player player);
        IReadOnlyList<Player> GetAll();
    }
}
