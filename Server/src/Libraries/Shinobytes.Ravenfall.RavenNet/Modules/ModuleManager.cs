using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class ModuleManager : IModuleManager
    {
        private readonly ConcurrentDictionary<string, IModule> modules = new ConcurrentDictionary<string, IModule>();

        public T AddModule<T>(T instance) where T : IModule
        {
            this.modules[instance.Name] = instance;
            return instance;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T)this.modules.Values.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public IModule GetModule(string name)
        {
            if (this.modules.TryGetValue(name, out var module)) return module;
            return null;
        }
        public void Dispose()
        {
            foreach (IDisposable module in modules.Values)
            {
                module.Dispose();
            }
        }
    }
}
