namespace RavenfallServer.Packets
{
    public class PlayerObjectActionResponse
    {
        public const short OpCode = 8;
        public int PlayerId { get; set; }
        public int ObjectServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
        public byte Status { get; set; }
    }
}
