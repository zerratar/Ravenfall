using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;

public class NpcTradeAction : EntityAction
{
    private readonly IWorldProcessor worldProcessor;
    private readonly IItemProvider itemProvider;
    private readonly INpcProvider npcProvider;
    private readonly IPlayerInventoryProvider inventoryProvider;
    private readonly INpcShopInventoryProvider shopInventoryProvider;

    public NpcTradeAction(
        IWorldProcessor worldProcessor,
        IItemProvider itemProvider,
        INpcProvider npcProvider,
        IPlayerInventoryProvider inventoryProvider,
        INpcShopInventoryProvider shopInventoryProvider)
        : base(9, "Npc Trade")
    {
        this.worldProcessor = worldProcessor;
        this.itemProvider = itemProvider;
        this.npcProvider = npcProvider;
        this.inventoryProvider = inventoryProvider;
        this.shopInventoryProvider = shopInventoryProvider;
    }

    public override bool Invoke(
        Player player,
        Entity obj,
        int parameterId)
    {
        if (!(obj is Npc npc))
        {
            return false;
        }

        var shopInventory = shopInventoryProvider.GetInventory(npc.Id);
        if (shopInventory == null)
        {
            return false;
        }

        worldProcessor.OpenTradeWindow(player, npc, "Test shop", shopInventory);
        return false;
    }
}
