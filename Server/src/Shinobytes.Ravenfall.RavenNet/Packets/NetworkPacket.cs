namespace Shinobytes.Ravenfall.RavenNet
{
    public class NetworkPacket
    {
        public short Id { get; set; }
        public object Data { get; set; }

        public bool TryGetValue<T>(out T result)
        {
            if (Data is T res)
            {
                result = res;
                return true;
            }

            result = default;
            return false;
        }
    }
}
