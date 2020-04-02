using RavenfallServer.Objects;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;

namespace GameServer.ObjectActions
{
    public class RockPickAction : SceneObjectAction
    {
        private const int BronzePickAxeID = 1;
        private const int CopperRockID = 2;

        private const int actionTime = 2000;
        private const string SkillName = "Mining";

        private readonly IKernel kernel;
        private readonly IWorldProcessor worldProcessor;
        private readonly IObjectProvider objectProvider;
        private readonly IPlayerStatsProvider statsProvider;
        private readonly IItemProvider itemProvider;
        private readonly IRavenConnectionProvider connectionProvider;
        private readonly Random random = new Random();

        public RockPickAction(
            IKernel kernel,
            IWorldProcessor worldProcessor,
            IItemProvider itemProvider,
            IObjectProvider objectProvider,
            IPlayerStatsProvider statsProvider,
            IRavenConnectionProvider connectionProvider)
            : base(2, "RockPick")
        {
            this.kernel = kernel;
            this.worldProcessor = worldProcessor;
            this.itemProvider = itemProvider;
            this.objectProvider = objectProvider;
            this.statsProvider = statsProvider;
            this.connectionProvider = connectionProvider;
        }

        public override bool Invoke(Player player, SceneObject obj, int parameterId)
        {
            if (!(obj is CopperRockObject rock))
            {
                return false;
            }

            //if (rock.ObjectId == CopperRockID)
            //{
            //    return false;
            //}

            if (!objectProvider.AcquireObjectLock(rock, player))
            {
                return false;
            }

            var item = itemProvider.GetItemById(BronzePickAxeID);
            if (item != null)
            {
                worldProcessor.SetItemEquipState(player, item, true);
            }

            StartMiningAnimation(player, rock);
            return true;
        }

        private bool HandleObjectTick(Player player, CopperRockObject rock, TimeSpan totalTime, TimeSpan deltaTime)
        {
            if (!objectProvider.HasAcquiredObjectLock(rock, player))
            {
                StopMiningAnimation(player);
                return true;
            }

            var skill = statsProvider.GetStatByName(player.Id, SkillName);
            var chance = 0.05f + skill.EffectiveLevel * 0.05f;
            if (random.NextDouble() <= chance)
            {
                StartMiningAnimation(player, rock);
                return false;
            }

            var levelsGaiend = skill.AddExperience(rock.Experience);

            worldProcessor.UpdatePlayerStat(player, skill);

            if (levelsGaiend > 0)
            {
                worldProcessor.PlayerStatLevelUp(player, skill, levelsGaiend);
            }

            StopMiningAnimation(player);
            return true;
        }

        private void StartMiningAnimation(Player player, CopperRockObject tree)
        {
            worldProcessor.PlayAnimation(player, SkillName, true, true);
            worldProcessor.SetObjectTimeout(actionTime, player, tree, HandleObjectTick);
        }

        private void StopMiningAnimation(Player player)
        {
            var item = itemProvider.GetItemById(BronzePickAxeID);
            if (item != null)
            {
                worldProcessor.SetItemEquipState(player, item, false);
            }

            worldProcessor.PlayAnimation(player, SkillName, false);
            objectProvider.ReleaseObjectLocks(player);
        }
    }
}