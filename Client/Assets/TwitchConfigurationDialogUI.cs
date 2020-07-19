using Assets.Scripts.Twitch;
using System.Collections;
using UnityEngine;

public class TwitchConfigurationDialogUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputChannel;
    [SerializeField] private TMPro.TMP_InputField inputBot;
    [SerializeField] private TMPro.TMP_InputField inputToken;
    [SerializeField] private TwitchClient twitchClient;

    private TwitchOAuthTokenGenerator tokenGenerator = new TwitchOAuthTokenGenerator();
    private Settings tempSettings;

    // Start is called before the first frame update
    void Start()
    {
        tokenGenerator.AccessTokenReceived += TokenGenerator_AccessTokenReceived;
        gameObject.SetActive(false);
    }

    public void ShowHelp()
    {
        Application.OpenURL("https://www.ravenfall.stream/download");
    }

    public void ShowGenerateToken()
    {
        //Application.OpenURL("https://twitchtokengenerator.com/");
        StartCoroutine(tokenGenerator.StartAuthenticationProcess());
    }
    private void TokenGenerator_AccessTokenReceived(object sender, TwitchOAuthResult e)
    {
        inputToken.text = e.AccessToken;
        inputBot.text = e.User;
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

        gameObject.SetActive(true);

        StartCoroutine(UpdateInputs(tempSettings));
    }

    private IEnumerator UpdateInputs(Settings settings)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);

        inputChannel.text = settings.TwitchChannel;
        inputBot.text = settings.TwitchBotUsername;
        inputToken.text = inputToken.text;
    }

    private void OnDestroy()
    {
        tokenGenerator.Dispose();
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
