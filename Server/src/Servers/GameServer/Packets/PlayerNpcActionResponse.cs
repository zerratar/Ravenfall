namespace RavenfallServer.Packets
{
    public class PlayerNpcActionResponse
    {
        public const short OpCode = 33;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
        public byte Status { get; set; }
    }
}
