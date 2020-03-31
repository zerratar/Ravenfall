namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public interface IEntityStateChange<TEntity>
    {
        EntityState State { get; }
        TEntity Entity { get; }
    }
}
