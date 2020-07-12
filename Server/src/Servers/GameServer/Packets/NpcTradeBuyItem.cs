namespace RavenfallServer.Packets
{
    public class NpcTradeBuyItem
    {
        public const short OpCode = 37;
        public int NpcServerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}
