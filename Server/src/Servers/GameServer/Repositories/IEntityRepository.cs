using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface IEntityRepository<T>
    {
        IReadOnlyList<T> All();
    }
}
