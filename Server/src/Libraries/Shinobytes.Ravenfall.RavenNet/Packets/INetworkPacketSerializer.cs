using System.IO;

namespace Shinobytes.Ravenfall.RavenNet
{
    public interface INetworkPacketSerializer
    {
        byte[] Serialize(NetworkPacket packet);

        void Serialize(NetworkPacket packet, MessageWriter writer);
        NetworkPacket Deserialize(MessageReader reader);

        NetworkPacket Deserialize(byte[] data);
        NetworkPacket Deserialize(byte[] data, int packetSize);
        NetworkPacket Deserialize(BinaryReader br, int packetSize);
    }
}
