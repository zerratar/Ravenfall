using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shinobytes.Ravenfall.RavenNet.Core;

namespace Shinobytes.Ravenfall.Data.EntityFramework
{
    public class RavenfallDbContextProvider : IRavenfallDbContextProvider
    {
        private readonly AppSettings settings;

        public RavenfallDbContextProvider(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        public RavenfallDbContext Get()
        {
            var ctx = new RavenfallDbContext(settings.DbConnectionString);
            ctx.ChangeTracker.AutoDetectChangesEnabled = false;
            return ctx;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(settings.DbConnectionString);
        }
    }
}