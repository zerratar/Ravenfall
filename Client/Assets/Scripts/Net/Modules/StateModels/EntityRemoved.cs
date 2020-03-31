namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class EntityRemoved<TEntity> : IEntityStateChange<TEntity>
    {
        public EntityRemoved(TEntity entity)
        {
            Entity = entity;
            State = EntityState.Remove;
        }

        public EntityState State { get; }
        public TEntity Entity { get; }
    }
}
