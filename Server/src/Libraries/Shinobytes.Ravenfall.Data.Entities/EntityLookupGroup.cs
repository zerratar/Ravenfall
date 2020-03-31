using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.Data.Entities
{
    public class EntityLookupGroup<TModel, TKey>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TModel>> entities;
        private readonly Func<TModel, TKey> lookupKey;
        private readonly Func<TModel, TKey> itemKey;

        public EntityLookupGroup(ConcurrentDictionary<TKey, ConcurrentDictionary<TKey, TModel>> entities, Func<TModel, TKey> lookupKey, Func<TModel, TKey> itemKey)
        {
            this.entities = entities;
            this.lookupKey = lookupKey;
            this.itemKey = itemKey;
        }

        public ConcurrentDictionary<TKey, TModel> this[TKey key]
        {
            get
            {
                if (entities.TryGetValue(key, out var dict)) return dict;
                return null;
            }
        }

        public TModel this[TKey group, TKey item] => entities[group][item];

        public void Add(TModel entity)
        {
            var groupKey = lookupKey(entity);
            var key = itemKey(entity);

            if (!entities.ContainsKey(groupKey))
            {
                entities[groupKey] = new ConcurrentDictionary<TKey, TModel>();
            }

            if (entities.TryGetValue(groupKey, out var dict))
            {
                dict[key] = entity;
            }
        }

        public void Remove(TModel entity)
        {
            var groupKey = lookupKey(entity);
            var key = itemKey(entity);
            entities[groupKey].Remove(key, out _);
        }

        public void Update(TModel entity)
        {
            var groupKey = lookupKey(entity);
            var key = itemKey(entity);
            if (!entities.ContainsKey(groupKey) || !entities[groupKey].ContainsKey(key))
            {
                MoveEntitySlow(entity);
            }
        }

        private void MoveEntitySlow(TModel entity)
        {
            var groupFound = false;
            var oldKey = default(TKey);
            foreach (var groups in entities)
            {
                foreach (var value in groups.Value)
                {
                    if (ReferenceEquals(value.Value, entity))
                    {
                        oldKey = value.Key;
                        groupFound = true;
                        break;
                    }
                }

                if (groupFound)
                {
                    groups.Value.TryRemove(oldKey, out _);
                    Add(entity);
                    return;
                }
            }
        }
    }
}