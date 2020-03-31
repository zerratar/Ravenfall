using System;

namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public interface IMessageBus
    {
        void Send<T>(string key, T message);
        IMessageBusSubscription Subscribe<T>(string key, Action<T> onMessage);
    }
}
