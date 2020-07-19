using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Twitch
{
    public class TwitchOAuthTokenGenerator : IDisposable
    {
        private const string TwitchClientID = "757vrtjoawg2rtquprnfb35nqah1w4";
        private const string TwitchRedirectUri = "https://id.twitch.tv/oauth2/authorize";

        public event EventHandler<TwitchOAuthResult> AccessTokenReceived;
        private HttpListener listener;
        private bool disposed;

        public IEnumerator StartAuthenticationProcess()
        {
            var validationToken = GenerateValidationToken();
            var authUrl = GetAccessTokenRequestUrl(validationToken);
            TwitchOAuthResult accessToken = null;

            GetAccessToken((token, user, userId) =>
            {
                accessToken = new TwitchOAuthResult();
                accessToken.AccessToken = token;
                accessToken.User = user;
                accessToken.UserID = userId;
            });

            Application.OpenURL(authUrl);

            yield return new WaitUntil(() => accessToken != null);


            AccessTokenReceived?.Invoke(this, accessToken);

            Debug.Log(accessToken);
        }

        private string GetAccessTokenRequestUrl(string validationToken)
        {
            return
                TwitchRedirectUri + "?response_type=code" +
                $"&client_id={TwitchClientID}" +
                $"&redirect_uri=https://www.ravenfall.stream/api/twitch/authorize" +
                $"&scope=user_read+channel:moderate+chat:edit+chat:read" +
                $"&state={validationToken}&force_verify=true";
        }

        private string GenerateValidationToken()
        {
            return Convert.ToBase64String(Enumerable.Range(0, 20).Select(x => (byte)((byte)(UnityEngine.Random.value * ((byte)'z' - (byte)'a')) + (byte)'a')).ToArray());
        }

        private void GetAccessToken(Action<string, string, string> onTokenReceived)
        {
            new Thread(async () =>
            {
                try
                {
                    if (listener == null || !listener.IsListening)
                    {
                        listener = new HttpListener();
                        listener.Prefixes.Add("http://*:8182/");
                        listener.Start();
                    }

                    try
                    {
                        var context = listener.GetContext();
                        var req = context.Request;
                        var res = context.Response;
                        var state = req.QueryString["state"];
                        var accessToken = req.QueryString["token"];

                        var user = req.QueryString["user"];
                        var userId = req.QueryString["id"];

                        if (string.IsNullOrEmpty(user))
                        {
                            var result = await ValidateOAuthAsync(accessToken);
                            if (result != null)
                            {
                                user = result.Login;
                                userId = result.UserID;
                            }
                        }

                        onTokenReceived(accessToken, user, userId);

                        res.StatusCode = 200;
                        res.Close();
                    }
                    catch (Exception exc)
                    {
                        Debug.LogError(exc);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }).Start();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (listener != null)
            {
                try { listener.Abort(); } catch { }
                try { listener.Close(); } catch { }
                try { listener.Stop(); } catch { }
            }
        }

       
        private async Task<TwitchValidateResponse> ValidateOAuthAsync(string access_token)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create("https://id.twitch.tv/oauth2/validate");
            req.Method = "GET";
            req.Accept = "application/vnd.twitchtv.v5+json";
            req.Headers["Authorization"] = $"OAuth {access_token}";
            req.Headers["Client-ID"] = TwitchClientID;

            try
            {
                using (var res = await req.GetResponseAsync())
                using (var stream = res.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<TwitchValidateResponse>(
                            await reader.ReadToEndAsync());
                }
            }
            catch (WebException we)
            {
                var resp = we.Response as HttpWebResponse;
                if (resp != null)
                {
                    using (var stream = resp.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorText = await reader.ReadToEndAsync();
                        Debug.LogError(errorText);

                    }
                }
                return null;
            }
        }
    }

    public class TwitchValidateResponse
    {
        [JsonProperty("client_id")]
        public string ClientID { get; set; }
        public string Login { get; set; }
        public string[] Scopes { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
    }

    public class TwitchOAuthResult
    {
        public string AccessToken { get; set; }
        public string UserID { get; set; }
        public string User { get; set; }
    }
}
