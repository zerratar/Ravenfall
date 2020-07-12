using GameServer.Repositories;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RavenfallServer.Providers
{
    public partial class NpcProvider : EntityProvider, INpcProvider
    {
        private readonly List<Npc> entities = new List<Npc>();
        private readonly object mutex = new object();
        private readonly IoC ioc;
        private readonly INpcRepository npcRepository;
        private readonly IEntityActionsRepository actionRepo;
        private int index = 0;

        public NpcProvider(IoC ioc, INpcRepository npcRepo, IEntityActionsRepository actionRepo)
            : base(ioc, actionRepo)
        {
            this.ioc = ioc;
            this.npcRepository = npcRepo;
            this.actionRepo = actionRepo;

            AddNpcs();
            AddActions(EntityType.NPC);
        }

        private void AddNpcs()
        {
            var npcs = this.npcRepository.AllNpcs();

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