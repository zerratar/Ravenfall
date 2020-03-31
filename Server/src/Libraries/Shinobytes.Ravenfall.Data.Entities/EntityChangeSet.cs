using System;

namespace Shinobytes.Ravenfall.Data.Entities
{
    public class EntityChangeSet
    {
        public DateTime LastModified { get; set; }
        public EntityState State { get; set; }
        public IEntity Entity { get; set; }
    }
}