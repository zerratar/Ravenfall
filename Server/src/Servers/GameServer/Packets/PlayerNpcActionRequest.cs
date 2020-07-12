namespace RavenfallServer.Packets
{
    public class PlayerNpcActionRequest
    {
        public const short OpCode = 31;
        public int NpcServerId { get; set; }
        public int ActionId { get; set; }
        public int ParameterId { get; set; }
    }
}
