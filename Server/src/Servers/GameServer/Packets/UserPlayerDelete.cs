namespace RavenfallServer.Packets
{
    public class UserPlayerDelete
    {
        public const short OpCode = 21;
        public int PlayerId { get; set; }
    }
}
