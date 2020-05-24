using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Providers
{
    public interface IPlayerProvider
    {
        Player Get(string username);
        Player Get(int playerId);
        bool Remove(int playerId);
        IReadOnlyList<Player> GetAll();
        IReadOnlyList<Player> GetPlayers(User user);
        void Create(User user, string name, Appearance appearance);
    }
}
