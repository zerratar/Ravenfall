using UnityEngine;

[CreateAssetMenu(fileName = "New Player State Actions", menuName = "Game/Player State Actions")]
public class ServerPlayerAlignmentActions : ScriptableObject
{    
    public string Name;
    public PlayerAlignment Alignment;
    public ServerAction[] Actions;
}

public enum PlayerAlignment
{
    Neutral,
    Party,
    Enemy
}