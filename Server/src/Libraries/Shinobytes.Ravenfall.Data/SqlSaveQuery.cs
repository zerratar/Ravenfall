using System.Data.SqlClient;

namespace Shinobytes.Ravenfall.Data
{
    public class SqlSaveQuery
    {
        public SqlSaveQuery(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}