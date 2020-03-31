using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets
{
    public interface INetworkPacketController
    {
        INetworkPacketController Register(Type packetType, Type packetHandlerType, short packetId);
        INetworkPacketController Register<TPacket, TPacketHandler>() where TPacketHandler : INetworkPacketHandler<TPacket>;
        INetworkPacketController Register<TPacket, TPacketHandler>(short packetId) where TPacketHandler : INetworkPacketHandler<TPacket>;
        INetworkPacketController Register<TPacket>();
        INetworkPacketController Register<TPacket>(short id);

        void Handle(IRavenNetworkConnection connection, NetworkPacket packet, SendOption sendOption);
        void HandlePacketData(IRavenNetworkConnection connection, MessageReader message, SendOption sendOption);
        void Send<T>(Connection connection, short packetId, T packet, SendOption sendOption);
        void Send<T>(Connection connection, T packet, SendOption sendOption);
        void AddFilter<TResponse>(Func<TResponse, bool> filter);
    }
}