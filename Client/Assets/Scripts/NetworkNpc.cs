using System;
using UnityEngine;

public class NetworkNpc : MonoBehaviour
{
    [SerializeField] private EntityNavigation movement;
    [SerializeField] private EntityAnimationHandler animationHandler;
    [SerializeField] private EntityStats entityStats;
    public int Id { get; set; }
    public int ServerId { get; set; }
    public ServerNpc Data { get; set; }
    public EntityNavigation Navigation => movement;
    public NpcAlignment Alignment { get; set; }

    public void SetAnimationState(string animationState, bool enabled, bool trigger, int action)
    {
        if (!animationHandler) return;
        animationHandler.SetAnimationState(animationState, enabled, trigger, action);
    }

    internal void SetHealth(int health, int maxHealth)
    {
        entityStats.SetHealth(health, maxHealth);
    }
}
