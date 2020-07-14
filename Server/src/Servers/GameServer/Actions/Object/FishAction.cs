using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;

public class FishAction : SkillObjectAction
{
    public FishAction(
        IWorldProcessor worldProcessor,
        IItemManager itemProvider,
        IGameSessionManager sessionManager,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
: base(3,
      "Fish",
      "Fishing",
      2000,
      worldProcessor,
      sessionManager,
      itemProvider,
      statsProvider,
      inventoryProvider)
    {
    }
}
