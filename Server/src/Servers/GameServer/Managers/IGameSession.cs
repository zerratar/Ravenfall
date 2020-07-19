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
        IStreamBot Bot { get; }
        bool IsOpenWorldSession { get; }
        /// <summary>
        /// The current session's host connection. Will be null for non-player hosted sessions.
        /// </summary>
        public PlayerConnection Host { get; }
        
        void AddPlayer(PlayerConnection player);
        void AddPlayer(Player player);
        void RemovePlayer(Player player);
        bool ContainsPlayer(Player player);

        /// <summary>
        /// Assigns a <see cref="IStreamBot"/> to monitor this game session.
        /// </summary>
        /// <param name="bot"></param>
        void AssignBot(IStreamBot bot);
        /// <summary>
        /// Unassigns the assigned <see cref="IStreamBot"/> from this session.
        /// </summary>
        /// <param name="bot"></param>
        void UnassignBot(IStreamBot bot);
    }
}