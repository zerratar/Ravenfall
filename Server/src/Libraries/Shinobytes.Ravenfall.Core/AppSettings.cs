namespace Shinobytes.Ravenfall.RavenNet.Core
{
    public class AppSettings
    {
        public string DbConnectionString { get; set; }
        public string TwitchAccessToken { get; set; }
        public string TwitchRefreshToken { get; set; }
        public string TwitchClientId { get; set; }
        public string TwitchClientSecret { get; set; }
        public string OriginBroadcasterId { get; set; }
    }
}