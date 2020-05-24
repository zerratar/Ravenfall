using System;
using RavenfallServer.Objects;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

public delegate bool ObjectTickHandler<TObject>(Player player, TObject obj, TimeSpan totalTime, TimeSpan deltaTime);

public interface IWorldProcessor
{
    void RemovePlayer(Player player);
    void AddPlayer(PlayerConnection connection);
    void PlayerObjectInteraction(Player player, SceneObject serverObject, SceneObjectAction action, int parameterId);
    void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0);
    void SetItemEquipState(Player player, Item item, bool v);
    void SendChatMessage(Player player, int channelID, string message);
    void UpdatePlayerStat(Player player, PlayerStat skill);
    void PlayerStatLevelUp(Player player, PlayerStat skill, int levelsGaiend);
    void AddPlayerItem(Player player, Item item, int amount = 1);
    void RemovePlayerItem(Player player, Item item, int amount = 1);

    void RegisterObjectTickUpdate<TObject>(
        Player player,
        TObject tree,
        ObjectTickHandler<TObject> handleObjectTick)
        where TObject : SceneObject;

    ITimeoutHandle SetObjectTimeout<TObject>(
        int milliseconds,
        Player player,
        TObject obj,
        ObjectTickHandler<TObject> handleObjectTick)
        where TObject : SceneObject;

    void ClearObjectTimeout(ITimeoutHandle handle);
}
