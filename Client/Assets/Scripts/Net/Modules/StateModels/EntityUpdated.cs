namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class EntityUpdated<TEntity> : IEntityStateChange<TEntity>
    {
        public EntityUpdated(TEntity entity)
        {
            Entity = entity;
            State = EntityState.Update;
        }

        public EntityState State { get; }
        public TEntity Entity { get; }
    }
}
