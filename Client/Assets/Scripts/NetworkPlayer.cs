using System;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private EntityNavigation movement;
    [SerializeField] private EntityAnimationHandler animationHandler;
    [SerializeField] private EntityEquipmentHandler equipmentHandler;
    [SerializeField] private EntityStats entityStats;

    [SerializeField] private float positionUpdateInterval = 0.2f;
    [SerializeField] private float objectInteractionRange = 1.5f;

    private NetworkClient networkClient;
    private NetworkObject targetInteractionObject;

    private float playerPositionUpdateTimer;
    private Vector3 lastPushedPosition;

    public bool IsMe { get; set; }
    public int Id { get; set; }
    public EntityNavigation Navigation => movement;
    public PlayerManager PlayerManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        networkClient = FindObjectOfType<NetworkClient>();
    }

    // Update is called once per frame
    void Update()
    {
        SendPosition();
        UpdateObjectInteraction();
    }

    private void UpdateObjectInteraction()
    {
        if (!targetInteractionObject) return;
        var obj = targetInteractionObject;
        GetObjectAction(obj, out var distance, out var action, out var interactionRange);
        if (distance <= interactionRange)
        {
            networkClient.SendObjectAction(obj.ServerId, action.Id, 0);
            targetInteractionObject = null;
        }
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

    internal void MoveToAndInteractWith(NetworkObject obj)
    {
        if (!networkClient.Auth.Authenticated) return;
        targetInteractionObject = obj;

        GetObjectAction(obj, out var distance, out var action, out var interactionRange);

        if (distance > objectInteractionRange)
        {
            var direction = (transform.position - obj.transform.position).normalized;
            var targetMoveToPosition = obj.transform.position + ((interactionRange / 2.1f) * direction);
            networkClient.MoveTo(targetMoveToPosition);
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
        entityStats.PlayLevelUpAnimation(skill, gainedLevels);
    }

    public void MoveTo(Vector3 destination)
    {
        if (!networkClient.Auth.Authenticated) return;
        networkClient.MoveTo(destination);
    }

    private void GetObjectAction(NetworkObject obj, out float distance, out ServerAction action, out float interactionRange)
    {
        var data = obj.ObjectData;
        distance = Vector3.Distance(obj.transform.position, transform.position);
        action = data.Actions[0];
        interactionRange = action.Range > 0 ? action.Range : data.InteractionRange > 0 ? data.InteractionRange : objectInteractionRange;
    }
}
