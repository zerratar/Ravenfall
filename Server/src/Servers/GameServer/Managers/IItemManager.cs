using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer.Managers
{
    public interface IItemManager
    {
        Item GetItemById(int itemId);
    }
}