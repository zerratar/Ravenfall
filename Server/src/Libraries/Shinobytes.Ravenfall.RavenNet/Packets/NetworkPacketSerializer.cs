using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;
using System.IO;

namespace Shinobytes.Ravenfall.RavenNet
{
    public class NetworkPacketSerializer : INetworkPacketSerializer
    {
        private readonly ILogger logger;
        private readonly INetworkPacketTypeRegistry packetLookup;
        private readonly IBinarySerializer binarySerializer;
        private const int HeaderSize = sizeof(short);
        public NetworkPacketSerializer(ILogger logger, INetworkPacketTypeRegistry packetLookup, IBinarySerializer binarySerializer)
        {
            this.logger = logger;
            this.packetLookup = packetLookup;
            this.binarySerializer = binarySerializer;
        }

        public NetworkPacket Deserialize(MessageReader br)
        {
            var packetSize = br.ReadInt32();
            var packet = new NetworkPacket();
            packet.Id = br.ReadInt16();
            var payload = br.ReadBytes(packetSize - HeaderSize);//dataSize
            if (packetLookup.TryGetValue(packet.Id, out var targetType))
            {
                packet.Data = binarySerializer.Deserialize(payload, targetType);
                return packet;
            }

            if (logger != null)
            {
                logger.Error($"Unable to deserialize packet. No type lookups registered for packet ID ({packet.Id}).");
            }

            return null;
            //throw new Exception($"Unable to deserialize packet. No type lookups registered for packet ID ({packet.Id}).");
        }

        public void Serialize(NetworkPacket packet, MessageWriter writer)
        {
            var bytes = Serialize(packet);
            writer.Write(bytes);
        }

        public NetworkPacket Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data, 0, data.Length))
            using (var br = new BinaryReader(ms))
            {
                var packetSize = br.ReadInt32();
                return Deserialize(br, packetSize);
            }
        }

        public NetworkPacket Deserialize(byte[] data, int packetSize)
        {
            using (var ms = new MemoryStream(data, 0, packetSize))
            using (var br = new BinaryReader(ms))
            {
                return Deserialize(br, packetSize);
            }
        }

        public NetworkPacket Deserialize(BinaryReader br, int packetSize)
        {
            var packet = new NetworkPacket();
            packet.Id = br.ReadInt16();
            var payload = br.ReadBytes(packetSize - HeaderSize);//dataSize
            if (packetLookup.TryGetValue(packet.Id, out var targetType))
            {
                packet.Data = binarySerializer.Deserialize(payload, targetType);
                return packet;
            }

            packet.Data = payload;
            return packet;
        }

        public byte[] Serialize(NetworkPacket packet)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                var body = binarySerializer.Serialize(packet.Data);
                bw.Write(body.Length + HeaderSize);
                bw.Write(packet.Id);
                bw.Write(body);
                return ms.ToArray();
            }
        }

    }
}
