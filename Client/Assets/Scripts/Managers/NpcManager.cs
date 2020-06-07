using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private ServerNpc[] spawnableNpcs;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform npcContainer;

    private readonly List<NetworkNpc> npcs = new List<NetworkNpc>();

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        spawnableNpcs = Resources.LoadAll<ServerNpc>("Data/NPCs");
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        npcs.Clear();
        npcContainer = GameObject.FindGameObjectWithTag("NPCContainer")?.transform;
    }

    internal void OnNpcAnimationStateChanged(Npc entity, string animationState, bool enabled, bool trigger, int action)
    {
        Debug.Log("OnNpcAnimationStateChanged");
        var npc = npcs.FirstOrDefault(x => x.Id == entity.Id);
        if (!npc)
        {
            Debug.LogError("Trying to play an animation on a npc that does not exist. ID: " + entity.Id);
            return;
        }

        npc.SetAnimationState(animationState, enabled, trigger, action);
    }

    internal void OnNpcAdded(Npc entity)
    {
        var npc = Instantiate(npcPrefab, npcContainer).GetComponent<NetworkNpc>();
        npc.Data = GetNpcData(entity);        
        npc.ServerId = entity.Id;
        npc.Alignment = npc.Data.Alignment;
        npc.name = npc.Data.Name;
        npc.transform.position = entity.Position;

        Instantiate(npc.Data.Model, npc.transform);
        npcs.Add(npc);
    }

    internal void OnNpcUpdated(Npc entity)
    {
    }

    internal void OnNpcRemoved(Npc entity)
    {
        var obj = npcs.FirstOrDefault(x => x.ServerId == entity.Id);
        if (!obj) return;
        if (npcs.Remove(obj))
        {
            Destroy(obj.gameObject);
        }
    }

    internal void OnNpcMove(Npc entity)
    {
        var npc = npcs.FirstOrDefault(x => x.Id == entity.Id);
        if (!npc)
        {
            return;
        }

        npc.Navigation.MoveTo(entity.Position, entity.Destination, false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ServerNpc GetNpcData(Npc npc)
    {
        return spawnableNpcs.FirstOrDefault(x => x.Id == npc.Id);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ServerNpc GetNpcData(int id)
    {
        return spawnableNpcs.FirstOrDefault(x => x.Id == id);
    }


    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
}
