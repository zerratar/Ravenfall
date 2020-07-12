namespace RavenfallServer.Packets
{
    public class NpcTradeSellItem
    {
        public const short OpCode = 36;
        public int NpcServerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}
