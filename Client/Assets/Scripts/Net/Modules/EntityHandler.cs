using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public abstract class EntityHandler<TEntity> : IModule
    {
        protected readonly ConcurrentQueue<IEntityStateChange<TEntity>> Changes = new ConcurrentQueue<IEntityStateChange<TEntity>>();
        protected readonly List<TEntity> Entities = new List<TEntity>();
        protected readonly object SyncRoot = new object();
        private readonly Func<TEntity, TEntity, bool> idComparison;

        public abstract string Name { get; }

        protected EntityHandler(Func<TEntity, TEntity, bool> idComparison)
        {
            this.idComparison = idComparison;
        }

        public IEntityStateChange<TEntity> PollEvent()
        {
            if (Changes.TryDequeue(out var state)) return state;
            return null;
        }

        public void Remove(TEntity entity)
        {
            lock (SyncRoot)
            {
                var targetEntity = Entities.FirstOrDefault(x => idComparison(x, entity));
                if (targetEntity == null) return;
                if (Entities.Remove(targetEntity))
                {
                    Changes.Enqueue(new EntityRemoved<TEntity>(entity));
                }
            }
        }

        internal void Update(TEntity entity, bool entityMustExist)
        {
            lock (SyncRoot)
            {
                var targetEntity = Entities.FirstOrDefault(x => idComparison(x, entity));
                if (targetEntity == null && entityMustExist) return;
                Changes.Enqueue(new EntityUpdated<TEntity>(entity));
            }
        }

        public void Add(TEntity entity)
        {
            lock (SyncRoot)
            {
                Entities.Add(entity);
                Changes.Enqueue(new EntityAdded<TEntity>(entity));
            }
        }
    }
}
