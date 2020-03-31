using System;

namespace Shinobytes.Ravenfall.RavenNet
{
    public interface INetworkPacketTypeRegistry
    {
        bool TryGetId(Type targetType, out short id);
        bool TryGetValue(short id, out Type targetType);
        INetworkPacketTypeRegistry Register<T>(short id);
        INetworkPacketTypeRegistry Register(Type packetType, short id);
    }
}