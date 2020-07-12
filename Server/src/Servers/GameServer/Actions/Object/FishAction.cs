using RavenfallServer.Providers;

public class FishAction : SkillObjectAction
{
    public FishAction(
        IWorldProcessor worldProcessor,
        IItemProvider itemProvider,
        IObjectProvider objectProvider,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
: base(3,
      "Fish",
      "Fishing",
      2000,
      worldProcessor,
      itemProvider,
      objectProvider,
      statsProvider,
      inventoryProvider)
    {
    }
}
