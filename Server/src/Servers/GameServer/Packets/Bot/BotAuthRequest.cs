namespace RavenfallServer.Packets
{
    public class BotAuthRequest
    {
        public const short OpCode = 1000;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
