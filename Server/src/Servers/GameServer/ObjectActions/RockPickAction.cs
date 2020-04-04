using RavenfallServer.Providers;

namespace GameServer.ObjectActions
{
    public class RockPickAction : SkillObjectAction
    {
        public RockPickAction(
            IWorldProcessor worldProcessor,
            IItemProvider itemProvider,
            IObjectProvider objectProvider,
            IPlayerStatsProvider statsProvider,
            IPlayerInventoryProvider inventoryProvider)
            : base(2,
                  "RockPick",
                  "Mining",
                  2000,
                  worldProcessor,
                  itemProvider,
                  objectProvider,
                  statsProvider,
                  inventoryProvider)
        {
        }
    }
}