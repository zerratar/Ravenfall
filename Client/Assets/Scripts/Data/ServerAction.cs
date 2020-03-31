using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "Game/Action")]
public class ServerAction : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite BubbleIcon;
    public float Range;
}