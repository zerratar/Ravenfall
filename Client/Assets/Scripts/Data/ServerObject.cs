using UnityEngine;

[CreateAssetMenu(fileName = "New World Object", menuName = "Game/Object")]
public class ServerObject : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public ServerAction[] Actions;
    public float InteractionRange;

    public GameObject Model;
    public Sprite Icon;
}