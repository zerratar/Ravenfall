using TwitchLib.Client.Models;
using UnityEngine;

namespace Assets.Scripts.Twitch.Commands
{
    public class Join : ITwitchCommandHandler
    {
        // dependency injection works fine here
        // so add anything needed. For registering more stuff see IoCContainer.cs
        // command handlers does not need to be registered manually. so creating a new class and have the name
        // the same as the command or <Command Name>CommandHandler is also fine. Example: JoinCommandHandler also works.
        public Join() { }

        public void Handle(TwitchClient client, ChatCommand command)
        {
            Debug.Log("!join command used!");

            client.SendChatMessage("Unable to join this game. Streamer is playing in the Open World. You can only join a streamer hosted game. Do you want to play Ravenfall too? Download it from https://...");
        }
    }
}