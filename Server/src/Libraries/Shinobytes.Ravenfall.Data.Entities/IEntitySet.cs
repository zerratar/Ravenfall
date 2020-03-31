using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.Data.Entities
{
    public interface IEntitySet
    {
        ICollection<EntityChangeSet> Added { get; }
        ICollection<EntityChangeSet> Updated { get; }
        ICollection<EntityChangeSet> Removed { get; }
        DateTime LastModified { get; }
        void ClearChanges();
        void Clear(IReadOnlyList<IEntity> entities);
    }

    public interface IEntitySet<TModel, TKey> : IEntitySet
    {
        ICollection<TModel> Entities { get; }

        void Add(TModel model);
        void Remove(TModel model);
        //void Update(TModel model); // handle updates automatically, requires Entity<T> to be used.

        void AddRange(IEnumerable<TModel> models);
        void RemoveRange(IEnumerable<TModel> models);
        bool TryGet(TKey key, out TModel entity);

        TModel this[TKey key] { get; }
    }
}