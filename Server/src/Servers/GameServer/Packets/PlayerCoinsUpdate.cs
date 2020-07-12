namespace RavenfallServer.Packets
{
    public class PlayerCoinsUpdate
    {
        public const short OpCode = 38;
        public int PlayerId { get; set; }
        public long Coins { get; set; }
    }
}
