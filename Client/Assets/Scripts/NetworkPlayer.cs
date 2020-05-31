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
    private ObjectManager objectManager;

    private float playerPositionUpdateTimer;
    private UnityEngine.Vector3 lastPushedPosition;

    public bool IsMe { get; set; }
    public int Id { get; set; }
    public EntityNavigation Navigation => movement;
    public PlayerManager PlayerManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        networkClient = FindObjectOfType<NetworkClient>();
        objectManager = FindObjectOfType<ObjectManager>();
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SendPosition();
        UpdateObjectInteraction();
    }

    private void UpdateObjectInteraction()
    {
        if (targetInteractionObject == null) return;
        var obj = targetInteractionObject;
        GetObjectAction(obj, out var distance, out var action, out var interactionRange);
        if (distance <= interactionRange)
        {
            targetInteractionObject = null;
            // ignore examinations
            if (action.Id == 0)
            {
                uiManager.ChatPanel.OnExamine(obj.ObjectData.Description);
                return;
            }

            networkClient.SendObjectAction(obj.ServerId, action.Id, 0);
        }
    }

    internal void SetAppearance(Appearance appearance)
    {
        equipmentHandler.SetAppearance(appearance);
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


        GetObjectAction(obj, out var distance, out var action, out var interactionRange);

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

    private void GetObjectAction(ObjectInteraction obj, out float distance, out ServerAction action, out float interactionRange)
    {
        var data = obj.ObjectData;
        distance = UnityEngine.Vector3.Distance(obj.Position, transform.position);
        action = obj.ActionId == -1 ? data.Actions[0] : data.Actions.FirstOrDefault(x => x.Id == obj.ActionId);
        interactionRange = action.Range > 0 ? action.Range : data.InteractionRange > 0 ? data.InteractionRange : objectInteractionRange;
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
}

public class ObjectInteraction
{
    public NetworkObject NetworkObject { get; set; }
    public StaticObject StaticObject { get; set; }
    public ServerObject ObjectData => NetworkObject ? NetworkObject.ObjectData : StaticObject.ObjectData;
    public int ServerId => NetworkObject ? NetworkObject.ServerId : StaticObject.Instance + 1;
    public UnityEngine.Vector3 Position => NetworkObject ? NetworkObject.transform.position : StaticObject.Position;
    public int ActionId { get; set; }
    internal static ObjectInteraction Create(NetworkObject worldObject, int actionId = 0)
    {
        return new ObjectInteraction()
        {
            NetworkObject = worldObject,
            ActionId = actionId
        };
    }

    internal static ObjectInteraction Create(StaticObject staticObject, int actionId = 0)
    {
        return new ObjectInteraction()
        {
            StaticObject = staticObject,
            ActionId = actionId
        };
    }
}
