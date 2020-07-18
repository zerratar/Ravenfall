namespace RavenfallServer.Packets
{
    public class ConnectionKillSwitch
    {
        public const short OpCode = 24;
        public int Reason { get; set; }

        public static ConnectionKillSwitch MultipleLocations => new ConnectionKillSwitch { Reason = 0 };
        public static ConnectionKillSwitch Banned => new ConnectionKillSwitch { Reason = 1 };
    }
}
