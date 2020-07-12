namespace RavenfallServer.Packets
{
    public class NpcRemove
    {
        public const short OpCode = 28;
        public int ServerId { get; set; }
    }
}
