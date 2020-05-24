using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameServer.Repositories
{
    public class JsonBasedRepository<T> : IEntityRepository<T>
    {
        private const string RepositoryFolder = "repositories";
        private readonly object mutex = new object();
        private readonly string repositoryFile;
        private List<T> itemSource;


        public JsonBasedRepository()
        {
            repositoryFile = typeof(T).FullName + ".json";
            LoadRepository();
        }

        public IReadOnlyList<T> All()
        {
            lock (mutex)
            {
                return this.itemSource;
            }
        }

        private void LoadRepository()
        {
            lock (mutex)
            {
                var file = GetRepositoryFilePath();
                if (!System.IO.File.Exists(file))
                {
                    this.itemSource = new List<T>();
                    return;
                }

                var data = System.IO.File.ReadAllText(file);
                this.itemSource = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(data);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetRepositoryFilePath()
        {
            return System.IO.Path.Combine(RepositoryFolder, repositoryFile);
        }
    }
}
