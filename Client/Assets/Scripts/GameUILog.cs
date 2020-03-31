using UnityEngine;
public class GameUILog : MonoBehaviour, Shinobytes.Ravenfall.RavenNet.Core.ILogger
{
    [SerializeField] private TMPro.TextMeshProUGUI textLog;

    public void Debug(string message)
    {
        WriteLine("<color=#55ffff>" + message);
    }

    public void Error(string errorMessage)
    {
        WriteLine("<color=red>"+errorMessage);
    }

    public void Write(string message)
    {
        WriteLine(message);
    }

    public void WriteLine(string message)
    {
        if (!this.textLog) return;
        this.textLog.text = message;
    }
}
