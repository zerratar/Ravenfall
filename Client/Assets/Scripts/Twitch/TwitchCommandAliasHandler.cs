using Shinobytes.Ravenfall.RavenNet.Core;
using TwitchLib.Client.Models;

namespace Assets.Scripts.Twitch.Commands
{
    public abstract class TwitchCommandAliasHandler<T> : ITwitchCommandHandler where T : ITwitchCommandHandler
    {
        private readonly IoC ioc;
        public TwitchCommandAliasHandler(IoC ioc)
        {
            this.ioc = ioc;
        }
        public void Handle(TwitchClient client, ChatCommand command)
        {
            ioc.Resolve<T>().Handle(client, command);
        }
    }
}