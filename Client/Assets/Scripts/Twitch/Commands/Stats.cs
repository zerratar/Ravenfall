using TwitchLib.Client.Models;
using UnityEngine;

namespace Assets.Scripts.Twitch.Commands
{
    public class Stats : ITwitchCommandHandler
    {
        public void Handle(TwitchClient client, ChatCommand command)
        {
            Debug.Log("!stats command used!");

            var playerManager = GameObject.FindObjectOfType<PlayerManager>();

            // Check if we are playing on a hosted stream, are hosting a stream or open world
            // * if we are playing on the open world stream, the current player's stats should be checked.
            // * if we are playing on a hosted stream, the current player's stats should be checked.
            // * if we are hosting a stream, the caller's player's stats should be checked            

            var streamerName = client.Settings.TwitchChannel;
            var player = playerManager.Me;
            if (!player)
            {
                client.SendChatMessage($"{streamerName} is not playing on any characters yet.");
                return;
            }

            client.SendChatMessage($"{streamerName} is playing {player.name} and its stats are: {GetSkillText(player)}");
        }

        private string GetSkillText(NetworkPlayer player)
        {
            var stats = player.Stats.Stats;
            var statsString = "";
            for (var i = 0; i < stats.Length; ++i)
            {
                var stat = stats[i];
                statsString += $"{stat.Name}: {stat.Level}, ";
            }
            return statsString.Trim().Trim(',');
        }
    }
}