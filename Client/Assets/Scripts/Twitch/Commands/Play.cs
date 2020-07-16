using Shinobytes.Ravenfall.RavenNet.Core;

namespace Assets.Scripts.Twitch.Commands
{
    // command alias example, it will redirect everything through join
    public class Play : TwitchCommandAliasHandler<Join>
    {
        public Play(IoC ioc) : base(ioc) { }
    }
}