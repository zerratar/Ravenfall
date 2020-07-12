namespace RavenfallServer.Packets
{
    public class PlayerHealthChange
    {
        public const short OpCode = 39;
        public int TargetPlayerId { get; set; }
        public int PlayerId { get; set; }
        public int Delta { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }

}
