using System;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public class MessageBus : IMessageBus
    {
        private readonly object mutex = new object();
        private readonly List<Subscription> subscriptions = new List<Subscription>();

        public void Send<T>(string key, T message)
        {
            lock (mutex)
            {
                foreach (var sub in subscriptions.Where(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    sub.Invoke(message);
                }
            }
        }

        public IMessageBusSubscription Subscribe<T>(string key, Action<T> onMessage)
        {
            lock (mutex)
            {
                var messageBusSubscription = new Subscription(key, o => onMessage((T)o), this);
                this.subscriptions.Add(messageBusSubscription);
                return messageBusSubscription;
            }
        }

        private void Unsubscribe(IMessageBusSubscription subscription)
        {
            lock (mutex)
            {
                this.subscriptions.Remove(subscription as Subscription);
            }
        }

        private class Subscription : IMessageBusSubscription
        {
            private readonly Action<object> onMessage;
            private readonly MessageBus bus;

            public Subscription(string key, Action<object> onMessage, MessageBus bus)
            {
                this.Key = key;
                this.onMessage = onMessage;
                this.bus = bus;
            }

            public string Key { get; }

            public void Invoke<T>(T message)
            {
                onMessage?.Invoke(message);
            }

            public void Unsubscribe()
            {
                bus.Unsubscribe(this);
            }
        }
    }
}
