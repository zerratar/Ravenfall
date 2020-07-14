using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;

public class RockPickAction : SkillObjectAction
{
    public RockPickAction(
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IItemManager itemProvider,        
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
        : base(2,
              "RockPick",
              "Mining",
              2000,
              worldProcessor,
              sessionManager,
              itemProvider,
              statsProvider,
              inventoryProvider)
    {
    }
}
