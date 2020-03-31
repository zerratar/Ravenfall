namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class EntityAction<TEntity> : IEntityStateChange<TEntity>
    {
        public EntityAction(TEntity entity)
        {
            Entity = entity;
            State = EntityState.Action;
        }

        public EntityState State { get; }
        public TEntity Entity { get; }
    }
}
