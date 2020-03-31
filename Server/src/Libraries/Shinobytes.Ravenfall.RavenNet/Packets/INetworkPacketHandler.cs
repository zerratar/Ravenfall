namespace Shinobytes.Ravenfall.RavenNet.Packets
{
    public interface INetworkPacketHandler { }
    public interface INetworkPacketHandler<T> : INetworkPacketHandler
    {
        void Handle(T data, IRavenNetworkConnection connection, SendOption sendOption);
    }
}