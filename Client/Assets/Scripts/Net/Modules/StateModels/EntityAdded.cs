namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class EntityAdded<TEntity> : IEntityStateChange<TEntity>
    {
        public EntityAdded(TEntity entity)
        {
            Entity = entity;
            State = EntityState.Add;
        }

        public EntityState State { get; }
        public TEntity Entity { get; }
    }
}
