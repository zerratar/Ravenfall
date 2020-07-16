using Shinobytes.Ravenfall.RavenNet.Core;

namespace Assets.Scripts.Twitch.Commands
{
    public class Commands : TwitchCommandAliasHandler<Help>
    {
        public Commands(IoC ioc) : base(ioc) { }
    }
}