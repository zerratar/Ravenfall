using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Game/NPC")]
public class ServerNpc : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public NpcAlignment Alignment;
    public ServerAction[] Actions;
    public float InteractionRange;
    public GameObject Model;    
}

public enum NpcAlignment
{
    Neutral,
    Party,
    Enemy
}