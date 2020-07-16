using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private EntityNavigation movement;
    [SerializeField] private EntityAnimationHandler animationHandler;
    [SerializeField] private EntityEquipmentHandler equipmentHandler;
    [SerializeField] private EntityStats entityStats;
    [SerializeField] private EntityInventory inventory;

    [SerializeField] private float positionUpdateInterval = 0.2f;
    [SerializeField] private float objectInteractionRange = 1.5f;

    [SerializeField] private UIManager uiManager;

    private NetworkClient networkClient;
    private ObjectInteraction targetInteractionObject;
    private NpcInteraction targetInteractionNpc;

    private float playerPositionUpdateTimer;
    private UnityEngine.Vector3 lastPushedPosition;

    public bool IsMe { get; set; }
    public int Id { get; set; }
    public EntityNavigation Navigation => movement;
    public EntityInventory Inventory => inventory;
    public EntityStats Stats => entityStats;

    public PlayerManager PlayerManager { get; set; }

    public float ObjectInteractionRange => objectInteractionRange;

    public PlayerAlignment Alignment { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        networkClient = FindObjectOfType<NetworkClient>();
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SendPosition();
        UpdateObjectInteraction();
        UpdateNpcInteraction();
        UpdatePlayerInteraction();
        UpdateItemInteraction();
    }

    internal void ClearInteractionTargets()
    {
        targetInteractionObject = null;
        targetInteractionNpc = null;
    }

    private void UpdateItemInteraction()
    {
    }

    private void UpdatePlayerInteraction()
    {
    }

    private void UpdateNpcInteraction()
    {
        if (targetInteractionNpc == null) return;

        if (!targetInteractionNpc.TryGetAction(
            out var distance,
            out var action,
            out var interactionRange))
        {
            return;
        }

        if (distance <= interactionRange)
        {
            try
            {
                // ignore examinations
                if (action.Id == 0)
                {
                    uiManager.ChatPanel.OnExamine(targetInteractionNpc.NpcData.Description);
                    return;
                }

                networkClient.SendNpcAction(targetInteractionNpc.ServerId, action.Id, 0);
            }
            finally
            {
                targetInteractionNpc = null;
            }
        }
    }

    private void UpdateObjectInteraction()
    {
        if (targetInteractionObject == null) return;

        if (!targetInteractionObject.TryGetAction(
            out var distance,
            out var action,
            out var interactionRange))
        {
            return;
        }

        if (distance <= interactionRange)
        {
            try
            {
                // ignore examinations
                if (action.Id == 0)
                {
                    uiManager.ChatPanel.OnExamine(targetInteractionObject.ObjectData.Description);
                    return;
                }

                networkClient.SendObjectAction(targetInteractionObject.ServerId, action.Id, 0);
            }
            finally
            {
                targetInteractionObject = null;
            }
        }
    }

    internal void SetAppearance(Appearance appearance)
    {
        equipmentHandler.SetAppearance(appearance);
    }

    internal void SetHealth(int health, int maxHealth)
    {
        entityStats.GetStatByName("health").Set(health, maxHealth);
    }

    private void SendPosition()
    {
        playerPositionUpdateTimer -= Time.deltaTime;
        if (playerPositionUpdateTimer > 0f) return;
        if (lastPushedPosition != transform.position)
        {
            networkClient.SendPosition(transform.position);
            playerPositionUpdateTimer = positionUpdateInterval;
            lastPushedPosition = transform.position;
        }
    }

    internal void MoveToAndInteractWith(ObjectInteraction obj, bool running)
    {
        if (!networkClient.Auth.Authenticated) return;
        targetInteractionObject = obj;

        if (!obj.TryGetAction(
            out var distance,
            out var action,
            out var interactionRange))
        {
            return;
        }

        if (distance > interactionRange)
        {
            var direction = (transform.position - obj.Position).normalized;
            var targetMoveToPosition = obj.Position + ((interactionRange / 2.1f) * direction);
            //var targetMoveToPosition = obj.transform.position;
            networkClient.MoveTo(targetMoveToPosition, running);
        }
    }

    internal void MoveToAndInteractWith(NpcInteraction obj, bool running)
    {
        if (!networkClient.Auth.Authenticated) return;
        targetInteractionNpc = obj;

        if (!obj.TryGetAction(
            out var distance,
            out var action,
            out var interactionRange))
        {
            return;
        }

        if (distance > interactionRange)
        {
            var direction = (transform.position - obj.Position).normalized;
            var targetMoveToPosition = obj.Position + ((interactionRange / 2.1f) * direction);
            //var targetMoveToPosition = obj.transform.position;
            networkClient.MoveTo(targetMoveToPosition, running);
        }
    }

    public void SetAnimationState(string animationState, bool enabled, bool trigger, int action)
    {
        if (!animationHandler) return;
        animationHandler.SetAnimationState(animationState, enabled, trigger, action);
    }

    public void SetEquipmentState(int itemId, bool equipped)
    {
        if (!equipmentHandler) return;
        equipmentHandler.SetEquipmentState(itemId, equipped);
    }

    internal void SetStats(decimal[] experience, int[] effectiveLevel)
    {
        if (!entityStats) return;
        entityStats.SetStats(experience, effectiveLevel);
    }

    public void UpdateStat(int skill, int level, int effectiveLevel, decimal experience)
    {
        entityStats.UpdateStat(skill, level, effectiveLevel, experience);
    }

    public void PlayLevelUpAnimation(int skill, int gainedLevels)
    {
        var stat = entityStats.PlayLevelUpAnimation(skill, gainedLevels);
        uiManager.ChatPanel.OnLevelUp(stat, gainedLevels);
    }

    public void MoveTo(UnityEngine.Vector3 destination, bool running)
    {
        if (!networkClient.Auth.Authenticated) return;
        networkClient.MoveTo(destination, running);
    }

    internal void AddInventoryItem(int itemId, int amount)
    {
        var item = inventory.AddItem(itemId, amount);
        uiManager.ChatPanel.OnItemAdd(item, amount);
    }

    internal void RemoveInventoryItem(int itemId, int amount)
    {
        var item = inventory.RemoveItem(itemId, amount);
        uiManager.ChatPanel.OnItemRemove(item, amount);
    }

    internal void SetInventoryItems(int[] itemId, long[] amount)
    {
        inventory.SetItems(itemId, amount);
    }

    internal void SetCoins(long amount)
    {
        inventory.SetCoins(amount);
    }
}
