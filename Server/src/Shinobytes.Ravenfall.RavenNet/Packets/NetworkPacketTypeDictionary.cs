using System;
using System.Collections.Concurrent;

namespace Shinobytes.Ravenfall.RavenNet
{
    public class NetworkPacketTypeRegistry : INetworkPacketTypeRegistry
    {
        private readonly ConcurrentDictionary<short, Type> dict = new ConcurrentDictionary<short, Type>();
        private readonly ConcurrentDictionary<Type, short> dictRev = new ConcurrentDictionary<Type, short>();


        public bool TryGetId(Type targetType, out short id)
        {
            return dictRev.TryGetValue(targetType, out id);
        }

        public bool TryGetValue(short id, out Type targetType)
        {
            return dict.TryGetValue(id, out targetType);

        }

        public INetworkPacketTypeRegistry Register<T>(short id)
        {
            this.dict[id] = typeof(T);
            this.dictRev[typeof(T)] = id;
            return this;
        }

        public INetworkPacketTypeRegistry Register(Type packetType, short id)
        {
            this.dict[id] = packetType;
            this.dictRev[packetType] = id;
            return this;
        }
    }
}
