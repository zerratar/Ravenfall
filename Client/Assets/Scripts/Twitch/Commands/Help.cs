using TwitchLib.Client.Models;
using UnityEngine;

namespace Assets.Scripts.Twitch.Commands
{
    public class Help : ITwitchCommandHandler
    {
        public void Handle(TwitchClient client, ChatCommand command)
        {
            Debug.Log("!help command used!");

            client.SendChatMessage($"No help is available at this time.");
        }
    }
}