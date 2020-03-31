using Microsoft.Data.SqlClient;

namespace Shinobytes.Ravenfall.Data.EntityFramework
{
    public interface IRavenfallDbContextProvider
    {
        SqlConnection GetConnection();
        RavenfallDbContext Get();
    }
}