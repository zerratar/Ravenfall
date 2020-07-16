using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchConfigurationDialogUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputChannel;
    [SerializeField] private TMPro.TMP_InputField inputBot;
    [SerializeField] private TMPro.TMP_InputField inputToken;
    [SerializeField] private TwitchClient twitchClient;

    private Settings tempSettings;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowHelp()
    {
        Application.OpenURL("https://www.ravenfall.stream/download");
    }

    public void ShowGenerateToken()
    {
        Application.OpenURL("https://twitchtokengenerator.com/");
    }

    internal void Show(Settings settings)
    {
        if (settings != null)
        {
            tempSettings = new Settings
            {
                TwitchBotAuthToken = settings.TwitchBotAuthToken,
                TwitchBotUsername = settings.TwitchBotUsername,
                TwitchChannel = settings.TwitchChannel
            };
        }
        else tempSettings = new Settings();
        inputChannel.text = settings.TwitchChannel;
        inputBot.text = settings.TwitchBotUsername;
        inputToken.text = inputToken.text;
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Save()
    {
        tempSettings.TwitchChannel = inputChannel.text;
        tempSettings.TwitchBotUsername = inputBot.text;
        tempSettings.TwitchBotAuthToken = inputToken.text;
        twitchClient.SaveSettings(this.tempSettings);
        gameObject.SetActive(false);
    }
}
