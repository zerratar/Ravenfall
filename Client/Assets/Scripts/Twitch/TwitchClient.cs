using Assets.Scripts;
using System;
using System.Runtime.CompilerServices;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{
    [SerializeField] private string defaultChannel = "";
    [SerializeField] private string defaultUser = "";
    [SerializeField] private string defaultToken = "";

    [SerializeField] private UIManager uiManager;

    private Client twitchClient;
    private Settings settings;

    private ITwitchCommandController commandController;

    public Settings Settings => settings;

    // Start is called before the first frame update
    void Start()
    {
        commandController = IoCContainer.Instance.Resolve<ITwitchCommandController>();
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();

        LoadSettings();

        if (CheckForBadSettings())
        {
            uiManager.ShowTwitchConfigurationDialog(this.settings);
            return;
        }

        Connect();
    }

    public void SendChatMessage(string message)
    {
        twitchClient.SendMessage(this.settings.TwitchChannel, message);
    }

    public void SaveSettings(Settings newSettings)
    {
        if (!SettingsModified(settings, newSettings))
        {
            if (twitchClient == null || !twitchClient.IsConnected)
            {
                Connect();
            }
            return;
        }

        this.settings = newSettings;
        var settingsFilePath = Application.persistentDataPath + "/settings.json";
        var content = Newtonsoft.Json.JsonConvert.SerializeObject(newSettings);
        System.IO.File.WriteAllText(settingsFilePath, content);
        Connect();
    }

    private bool SettingsModified(Settings settings, Settings newSettings)
    {
        return settings.TwitchBotAuthToken != newSettings.TwitchBotAuthToken ||
               settings.TwitchBotUsername != newSettings.TwitchBotUsername ||
               settings.TwitchChannel != newSettings.TwitchChannel;
    }

    private void LoadSettings()
    {
        var settingsFilePath = Application.persistentDataPath + "/settings.json";
        if (!System.IO.File.Exists(settingsFilePath))
        {
            this.settings = new Settings
            {
                TwitchChannel = defaultChannel,
                TwitchBotAuthToken = defaultToken,
                TwitchBotUsername = defaultUser
            };
            return;
        }

        var settingsContent = System.IO.File.ReadAllText(settingsFilePath);
        this.settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(settingsContent);
    }

    private void Disconnect()
    {
        if (twitchClient == null || !twitchClient.IsConnected)
        {
            return;
        }

        twitchClient.Disconnect();
    }

    private void Connect()
    {

        if (CheckForBadSettings())
        {
            return;
        }

        if (twitchClient != null && twitchClient.IsConnected)
        {
            UnregisterEvents();
            Disconnect();
        }

        //Create Credentials instance
        ConnectionCredentials credentials = new ConnectionCredentials(settings.TwitchBotUsername, settings.TwitchBotAuthToken);

        // Create new instance of Chat Client
        twitchClient = new Client();

        // Initialize the client with the credentials instance, and setting a default channel to connect to.
        twitchClient.Initialize(credentials, settings.TwitchChannel);

        // Bind callbacks to events
        twitchClient.OnConnected += OnConnected;
        twitchClient.OnJoinedChannel += OnJoinedChannel;
        twitchClient.OnMessageReceived += OnMessageReceived;
        twitchClient.OnChatCommandReceived += OnChatCommandReceived;
        twitchClient.OnDisconnected += OnDisconnected;

        // Connect
        twitchClient.Connect();
    }

    private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
    {
        uiManager.OnTwitchConnectionLost();
    }

    private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
    {
        commandController.HandleCommand(this, e.Command);
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        Debug.Log("Joined Channel");
    }

    private void OnConnected(object sender, OnConnectedArgs e)
    {
        uiManager.OnTwitchConnectionEstablished();
        Debug.Log("Connected");
    }

    private void UnregisterEvents()
    {
        if (twitchClient != null)
        {
            // Bind callbacks to events
            twitchClient.OnConnected -= OnConnected;
            twitchClient.OnJoinedChannel -= OnJoinedChannel;
            twitchClient.OnMessageReceived -= OnMessageReceived;
            twitchClient.OnChatCommandReceived -= OnChatCommandReceived;
            twitchClient.OnDisconnected += OnDisconnected;
        }
    }

    private void OnDestroy()
    {
        Disconnect();
        UnregisterEvents();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckForBadSettings()
    {
        return settings == null || string.IsNullOrEmpty(settings.TwitchChannel) || string.IsNullOrEmpty(settings.TwitchBotAuthToken) || string.IsNullOrEmpty(settings.TwitchBotUsername);
    }
}

public class Settings
{
    public string TwitchChannel { get; set; }
    public string TwitchBotUsername { get; set; }
    public string TwitchBotAuthToken { get; set; }
}
