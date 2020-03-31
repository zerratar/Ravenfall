using System;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public interface IModuleManager : IDisposable
    {
        T AddModule<T>(T instance) where T : IModule;
        //T AddModule<T>() where T : IModule;
        T GetModule<T>() where T : IModule;
        IModule GetModule(string name);
    }
}
