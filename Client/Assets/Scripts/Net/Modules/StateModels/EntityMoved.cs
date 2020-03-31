namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class EntityMoved<TEntity> : IEntityStateChange<TEntity>
    {
        public EntityMoved(TEntity entity)
        {
            Entity = entity;
            State = EntityState.Move;
        }

        public EntityState State { get; }
        public TEntity Entity { get; }
    }
}
