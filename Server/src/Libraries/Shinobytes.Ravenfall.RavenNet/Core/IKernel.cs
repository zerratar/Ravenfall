using System;

namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public interface IKernel : IDisposable
    {
        void RegisterTickUpdate(Action<TimeSpan> update, TimeSpan timeSpan);
        ITimeoutHandle SetTimeout(Action action, int timeoutMilliseconds);
        void ClearTimeout(ITimeoutHandle discordBroadcast);

        void Start();
        void Stop();
        bool Started { get; }

    }
}
