namespace RavenfallServer.Packets
{
    public class NpcTradeOpenWindow
    {
        public const short OpCode = 34;
        public int PlayerId { get; set; }
        public int NpcServerId { get; set; }
        public string ShopName { get; set; }
        public int[] ItemId { get; set; }
        public int[] ItemPrice { get; set; }
        public int[] ItemStock { get; set; }
    }
}
