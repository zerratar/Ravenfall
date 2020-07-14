using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;

namespace GameServer.Managers
{
    public class GameSession : IGameSession
    {
        public GameSession(
            INpcManager npcManager, 
            IObjectManager objectManager)
        {
            Npcs = npcManager;
            Objects = objectManager;
            Items = new ItemManager();
            Players = new PlayerManager();
        }

        public IItemManager Items { get; private set; }
        public INpcManager Npcs { get; private set; }
        public IObjectManager Objects { get; private set; }
        public IPlayerManager Players { get; private set; }
        public PlayerConnection Host { get; private set; }

        public bool ContainsPlayer(Player player)
        {
            if (player == null) return false;
            return Players.GetAll().Any(x => x.Id == player.Id);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
            Objects.ReleaseLocks(player);
        }
    }
}