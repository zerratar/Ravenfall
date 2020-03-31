using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.Data
{
    public interface IQueryBuilder
    {
        SqlSaveQuery Build(EntityStoreItems saveData);
    }
}