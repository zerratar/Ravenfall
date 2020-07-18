namespace RavenfallServer.Packets
{
    public class BotAuthResponse
    {
        public const short OpCode = 1001;
        public int Status { get; set; }
        public byte[] SessionKeys { get; set; }
    }
}
