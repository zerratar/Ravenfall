using System;

namespace Shinobytes.Ravenfall.RavenNet.Serializers
{
    public interface IBinarySerializer
    {
        object Deserialize(byte[] data, Type type);
        byte[] Serialize(object data);
    }
}
