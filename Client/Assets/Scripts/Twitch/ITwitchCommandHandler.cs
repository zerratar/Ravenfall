using TwitchLib.Client.Models;
using UnityEngine;

public interface ITwitchCommandHandler
{
    void Handle(TwitchClient client, ChatCommand command);
}
