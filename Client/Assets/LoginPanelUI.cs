using System.Collections;
using UnityEngine;

public class LoginPanelUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputUsername;
    [SerializeField] private TMPro.TMP_InputField inputPassword;
    [SerializeField] private NetworkClient networkClient;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLoginClicked()
    {
        if (!networkClient)
        {
            Debug.LogError("No network client available :((");
            return;
        }

        StartCoroutine(Login(inputUsername.text, inputPassword.text));
    }

    private IEnumerator Login(string username, string password)
    {
        inputUsername.readOnly = true;
        inputPassword.readOnly = true;

        while (!networkClient.IsConnected)
        {
            networkClient.Connect();
            yield return null;
        }

        while (!networkClient.IsAuthenticated)
        {
            networkClient.Authenticate(username, password);
            yield return null;
        }

        inputUsername.readOnly = false;
        inputPassword.readOnly = false;
        gameObject.SetActive(false);
    }
}
