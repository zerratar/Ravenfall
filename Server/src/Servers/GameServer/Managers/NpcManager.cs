using GameServer.Repositories;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public partial class NpcManager : EntityManager, INpcManager
    {
        private readonly List<Npc> entities = new List<Npc>();
        private readonly object mutex = new object();
        private readonly INpcRepository npcRepository;
        private int index = 0;

        public INpcShopInventoryProvider Inventories { get; }

        public INpcStatsProvider Stats { get; }

        public INpcStateProvider States { get; }

        public NpcManager(
            IoC ioc,
            INpcRepository npcRepo,
            IItemManager itemManager,
            IEntityActionsRepository actionRepo)
            : base(ioc, actionRepo)
        {
            npcRepository = npcRepo;

            // Any Npc related stuff should be instanced per Session
            // and should therefor be removed from here.
            Stats = new NpcStatsProvider();
            States = new NpcStateProvider();
            Inventories = new NpcShopInventoryProvider(itemManager);

            AddNpcs();
            AddActions(EntityType.NPC);
        }

        private void AddNpcs()
        {
            var npcs = npcRepository.AllNpcs();

            foreach (var npc in npcs)
            {
                npc.Id = Interlocked.Increment(ref index);
                entities.Add(npc);
            }
        }

        public IReadOnlyList<Npc> GetAll()
        {
            lock (mutex)
            {
                return entities;
            }
        }

        public Npc Get(int npcServerId)
        {
            lock (mutex)
            {
                return entities.FirstOrDefault(x => x.Id == npcServerId);
            }
        }

        public EntityAction GetAction(Npc npc, int actionId)
        {
            if (entityActions.TryGetValue(npc.NpcId, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }

            return null;
        }
    }
}