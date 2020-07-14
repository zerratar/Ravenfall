using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface INpcShopInventoryProvider
    {
        ShopInventory GetInventory(int npcServerId);
    }
}