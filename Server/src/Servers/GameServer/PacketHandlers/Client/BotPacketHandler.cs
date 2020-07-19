using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public abstract class BotPacketHandler<T> : INetworkPacketHandler<T>
    {
        public void Handle(T data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            Handle(data, connection as BotConnection);
        }
        protected abstract void Handle(T data, BotConnection connection);
    }
}
