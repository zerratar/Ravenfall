using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.Managers
{
    public interface IGameSession
    {
        IPlayerManager Players { get; }
        INpcManager Npcs { get; }
        IItemManager Items { get; }
        IObjectManager Objects { get; }
        /// <summary>
        /// The current sesion's host connection. Will be null for non-player hosted sessions.
        /// </summary>
        public PlayerConnection Host { get; }
        void AddPlayer(Player player);
        void RemovePlayer(Player player);
        bool ContainsPlayer(Player player);
    }
}