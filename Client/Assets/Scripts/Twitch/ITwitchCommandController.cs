using TwitchLib.Client.Models;
using UnityEngine;

public interface ITwitchCommandController
{
    void HandleCommand(TwitchClient client, ChatCommand command);
}
