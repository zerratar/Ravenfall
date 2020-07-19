using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.Managers
{
    public interface IStreamBotManager
    {
        void Add(IStreamBot bot);
        void Remove(IStreamBot bot);
        IStreamBot GetMostAvailable();
    }
}
