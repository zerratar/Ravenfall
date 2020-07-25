using Assets.Scripts;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using System;
using System.Collections;
using System.Net;
using UnityEngine;

public class NetworkClient : MonoBehaviour
{
    [SerializeField] private int serverPort = 8133;
    [SerializeField] private string serverAddress = "51.89.117.205";
    [SerializeField] private Shinobytes.Ravenfall.RavenNet.Core.ILogger logger;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private NpcManager npcManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private UIManager uiManager;

    private volatile bool canAuthenticate = true;
    private volatile bool canConnect = true;

    private IRavenClient gameClient;

    public Authentication Auth { get; private set; }
    public PlayerHandler PlayerHandler { get; private set; }
    public NpcHandler NpcHandler { get; private set; }
    public ObjectHandler ObjectHandler { get; private set; }
    public CharacterHandler CharacterHandler { get; private set; }
    public ChatMessageHandler ChatMessageHandler { get; private set; }

    public void MoveTo(Shinobytes.Ravenfall.RavenNet.Models.Vector3 destination, bool running = false)
    {
        var from = playerManager.Me.transform.position;
        if (from == (UnityEngine.Vector3)destination) return;
        gameClient.Send(new PlayerMoveRequest
        {
            Position = from,
            Destination = destination,
            Running = running,
        }, SendOption.Reliable);
    }

    internal void SendChatMessage(int channelId, string message)
    {
        gameClient.Send(new ChatMessage
        {
            ChannelId = channelId,
            Message = message,
        }, SendOption.Reliable);
    }

    internal void SendDeleteCharacter(int id)
    {
        gameClient.Send(new UserPlayerDelete
        {
            PlayerId = id
        }, SendOption.Reliable);
    }

    internal void SendCreateCharacter(Player player)
    {
        gameClient.Send(new UserPlayerCreate
        {
            Name = player.Name,
            Appearance = player.Appearance
        }, SendOption.Reliable);
    }

    public void SendPosition(UnityEngine.Vector3 position)
    {
        gameClient.Send(new PlayerPositionUpdate
        {
            Position = position
        }, SendOption.None);
    }

    public void SendNpcAction(int npcServerId, int actionId, int actionParameterId)
    {
        gameClient.Send(new PlayerNpcActionRequest
        {
            NpcServerId = npcServerId,
            ActionId = actionId,
            ParameterId = actionParameterId
        }, SendOption.None);
    }

    internal void SendSellItem(int npcServerId, int itemId, int amount)
    {
        gameClient.Send(new NpcTradeSellItem
        {
            NpcServerId = npcServerId,
            ItemId = itemId,
            Amount = amount
        }, SendOption.Reliable);
    }

    internal void SendBuyItem(int npcServerId, int itemId, int amount)
    {
        gameClient.Send(new NpcTradeBuyItem
        {
            NpcServerId = npcServerId,
            ItemId = itemId,
            Amount = amount
        }, SendOption.Reliable);
    }

    public void SendObjectAction(int objectServerId, int actionId, int actionParameterId)
    {
        gameClient.Send(new PlayerObjectActionRequest
        {
            ObjectServerId = objectServerId,
            ActionId = actionId,
            ParameterId = actionParameterId
        }, SendOption.None);
    }

    internal void SendSelectCharacter(string sessionKey, int id)
    {
        if (string.IsNullOrEmpty(sessionKey))
        {
            sessionKey = Auth.Username;
        }

        gameClient.Send(new UserPlayerSelect
        {
            PlayerId = id,
            SessionKey = sessionKey
        }, SendOption.Reliable);
    }

    // Start is called before the first frame update
    private void Start()
    {
        logger = GameObject.FindObjectOfType<GameUILog>();
        gameClient = IoCContainer.Instance.Resolve<IRavenClient>();
        Auth = gameClient.Modules.GetModule<Authentication>();
        PlayerHandler = gameClient.Modules.GetModule<PlayerHandler>();
        ObjectHandler = gameClient.Modules.GetModule<ObjectHandler>();
        NpcHandler = gameClient.Modules.GetModule<NpcHandler>();
        CharacterHandler = gameClient.Modules.GetModule<CharacterHandler>();
        ChatMessageHandler = gameClient.Modules.GetModule<ChatMessageHandler>();
        //Connect();
    }

    public bool IsConnected => gameClient.IsConnected;
    public bool IsAuthenticated => Auth.Authenticated;

    // Update is called once per frame
    private void Update()
    {
        if (!gameClient.IsConnected)
        {
            //Connect();
            return;
        }

        if (!Auth.Authenticated)
        {
            //Authenticate();
            return;
        }

        HandlePlayerUpdates();
        HandleObjectUpdates();
        HandleNpcUpdates();
    }

    private void HandleNpcUpdates()
    {
        var stateChange = NpcHandler.PollEvent();
        if (stateChange == null) return;

        switch (stateChange)
        {
            case NpcHealthUpdated healthChange:
                npcManager.OnNpcHealthChanged(healthChange.Entity, healthChange.Health, healthChange.MaxHealth, healthChange.Delta);
                break;

            case NpcDied death:
                npcManager.OnNpcDeath(death.Entity);
                break;

            case NpcRespawned respawn:
                npcManager.OnNpcRespawn(respawn.Entity);
                break;

            case NpcAnimationStateUpdated animation:
                npcManager.OnNpcAnimationStateChanged(animation.Entity, animation.AnimationState, animation.Enabled, animation.Trigger, animation.Action);
                break;

            case EntityAdded<Npc> add:
                npcManager.OnNpcAdded(add.Entity);
                break;

            case EntityUpdated<Npc> updated:
                npcManager.OnNpcUpdated(updated.Entity);
                break;

            case EntityRemoved<Npc> removed:
                npcManager.OnNpcRemoved(removed.Entity);
                break;
        }
    }

    private void HandleObjectUpdates()
    {
        var stateChange = ObjectHandler.PollEvent();
        if (stateChange == null) return;

        switch (stateChange)
        {
            case EntityAdded<SceneObject> add:
                objectManager.OnObjectAdded(add.Entity);
                break;

            case EntityUpdated<SceneObject> updated:
                objectManager.OnObjectUpdated(updated.Entity);
                break;

            case EntityRemoved<SceneObject> removed:
                objectManager.OnObjectRemoved(removed.Entity);
                break;
        }
    }

    private void HandlePlayerUpdates()
    {
        var stateChange = PlayerHandler.PollEvent();
        if (stateChange == null) return;

        switch (stateChange)
        {
            case PlayerHealthUpdated healthChange:
                playerManager.OnPlayerHealthChanged(healthChange.Entity, healthChange.Health, healthChange.MaxHealth, healthChange.Delta);
                break;

            case OpenNpcTradeWindow npcTrade:
                uiManager.OnNpcTradeWindowOpen(npcTrade.Entity, npcTrade.NpcServerId, npcTrade.ShopName, npcTrade.ItemId, npcTrade.ItemPrice, npcTrade.ItemStock);
                break;

            case EntityAdded<Player> add:
                playerManager.OnPlayerAdded(add.Entity);
                break;

            case EntityRemoved<Player> removed:
                playerManager.OnPlayerRemoved(removed.Entity);
                break;

            case EntityMoved<Player> moved:
                playerManager.OnPlayerMove(moved.Entity);
                break;

            case PlayerNpcAction npcAction:
                playerManager.OnPlayerNpcAction(npcAction.Entity, npcAction.NpcId, npcAction.ActionType, npcAction.ParameterId, npcAction.Status);
                break;

            case PlayerObjectAction action:
                playerManager.OnPlayerObjectAction(action.Entity, action.ObjectId, action.ActionType, action.ParameterId, action.Status);
                break;

            case PlayerAnimationStateUpdated animation:
                playerManager.OnPlayerAnimationStateChanged(animation.Entity, animation.AnimationState, animation.Enabled, animation.Trigger, animation.Action);
                break;

            case PlayerEquipmentStateUpdated equipmentState:
                playerManager.OnPlayerEquipmentStateChanged(equipmentState.Entity, equipmentState.ItemId, equipmentState.Equipped);
                break;

            case PlayerStatUpdated statsUpdated:
                playerManager.OnPlayerStatUpdated(statsUpdated.Entity, statsUpdated.Skill, statsUpdated.Level, statsUpdated.Experience);
                break;

            case PlayerStatsUpdated allStats:
                playerManager.OnPlayerStatsUpdated(allStats.Entity);
                break;

            case PlayerLeveledUp levelUp:
                playerManager.OnPlayerLevelUp(levelUp.Entity, levelUp.Skill, levelUp.GainedLevels);
                break;

            case PlayerItemAdded itemAdded:
                playerManager.OnPlayerItemAdded(itemAdded.Entity, itemAdded.ItemId, itemAdded.Amount);
                break;

            case PlayerItemRemoved itemRemoved:
                playerManager.OnPlayerItemRemoved(itemRemoved.Entity, itemRemoved.ItemId, itemRemoved.Amount);
                break;
            case PlayerInventoryUpdated inventoryUpdated:
                playerManager.OnPlayerInventoryUpdated(inventoryUpdated.Entity, inventoryUpdated.Coins, inventoryUpdated.ItemId, inventoryUpdated.Amount);
                break;
        }
    }

    public void Authenticate(string username, string password)
    {
        if (Auth.Authenticating || !canAuthenticate) return;
        StartCoroutine(AuthenticateWithServer(username, password));
    }

    public void Connect()
    {
        if (gameClient.IsConnecting || gameClient.IsConnected || !canConnect) return;
        StartCoroutine(ConnectToServer());
    }

    private IEnumerator AuthenticateWithServer(string username, string password)
    {
        canAuthenticate = false;
        try
        {
            Log("Authenticating with server...");

            //Auth.Authenticate("player" + UnityEngine.Random.Range(1, 9999).ToString("0000"), "wowowow");

            Auth.Authenticate(username, password);

            while (Auth.Authenticating)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (Auth.Authenticated)
            {
                LogDebug("Authenticated");
                yield break;
            }

            LogError("Authentication failed.");
        }
        finally
        {
            canAuthenticate = true;
        }
    }

    private IEnumerator ConnectToServer()
    {
        canConnect = false;

        ResetState();
        try
        {
            Log("Connecting to server...");
            gameClient.ConnectAsync(TryGetServerAddress(), serverPort);

            while (gameClient.IsConnecting)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (gameClient.IsConnected)
            {
                LogDebug("Connected to the server");
                yield break;
            }

            LogError("Unable to connect to server");
            yield return new WaitForSeconds(1);
        }
        finally
        {
            canConnect = true;
        }
    }

    private void ResetState()
    {
        Auth.Reset();
        playerManager.ResetState();
        objectManager.ResetState();
    }

    private IPAddress TryGetServerAddress()
    {
        if (IPAddress.TryParse(serverAddress, out var address))
        {
            return address;
        }

        return IPAddress.Loopback;
    }

    private void Log(string message)
    {
        if (logger != null) logger.WriteLine(message);
    }

    private void LogDebug(string message)
    {
        if (logger != null) logger.Debug(message);
    }

    private void LogError(string message)
    {
        if (logger != null) logger.Error(message);
    }

    private void OnApplicationQuit()
    {
        gameClient.Dispose();
    }
}
