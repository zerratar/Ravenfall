using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface IPlayerInventoryProvider
    {
        Inventory GetInventory(int id);
    }
}