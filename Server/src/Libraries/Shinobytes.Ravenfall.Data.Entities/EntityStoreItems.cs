using System.Collections.Generic;
using Shinobytes.Ravenfall.Data.Entities;
namespace Shinobytes.Ravenfall.Data.Entities
{
    public class EntityStoreItems
    {
        public EntityStoreItems(EntityState state, IReadOnlyList<IEntity> entities)
        {
            State = state;
            Entities = entities;
        }

        public EntityState State { get; }
        public IReadOnlyList<IEntity> Entities { get; }
    }
}