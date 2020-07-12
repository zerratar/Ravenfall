namespace RavenfallServer.Packets
{
    public class NpcTradeUpdateStock
    {
        public const short OpCode = 35;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public int[] ItemId { get; set; }
        public int[] ItemPrice { get; set; }
        public int[] ItemStock { get; set; }
    }
}
