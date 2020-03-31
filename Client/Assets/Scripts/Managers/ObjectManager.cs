using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Shinobytes.Ravenfall.RavenNet.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private ServerObject[] spawnableObjects;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Transform objectContainer;

    private readonly List<NetworkObject> objects = new List<NetworkObject>();
    void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    internal void OnObjectAdded(SceneObject entity)
    {
        var networkObject = Instantiate(objectPrefab, objectContainer).GetComponent<NetworkObject>();
        networkObject.ObjectData = GetObjectData(entity);
        networkObject.ServerId = entity.Id;
        networkObject.name = networkObject.ObjectData.Name;
        networkObject.transform.position = entity.Position;

        Instantiate(networkObject.ObjectData.Model, networkObject.transform);
        objects.Add(networkObject);
    }

    internal void OnObjectUpdated(SceneObject entity)
    {
        Debug.Log("OnObjectUpdated");
        var networkObject = objects.FirstOrDefault(x => x.ServerId == entity.Id);
        if (!networkObject) return;

        var oldObjectId = networkObject.ObjectData.Id;

        networkObject.ObjectData = GetObjectData(entity);
        networkObject.name = networkObject.ObjectData.Name;
        networkObject.transform.position = entity.Position;

        if (entity.ObjectId != oldObjectId)
        {
            var model = networkObject.transform.GetChild(0);
            Destroy(model.gameObject);
            Instantiate(networkObject.ObjectData.Model, networkObject.transform);
        }
    }

    internal void OnObjectRemoved(SceneObject entity)
    {
        Debug.Log("OnObjectRemoved");
        var obj = objects.FirstOrDefault(x => x.ServerId == entity.Id);
        if (!obj) return;
        if (objects.Remove(obj))
        {
            Destroy(obj.gameObject);
        }
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        objects.Clear();
        objectContainer = GameObject.FindGameObjectWithTag("ObjectContainer")?.transform;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ServerObject GetObjectData(SceneObject entity)
    {
        return spawnableObjects.FirstOrDefault(x => x.Id == entity.ObjectId);
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    internal void Reset()
    {
    }

    internal void ResetState()
    {
        foreach (var obj in objects)
        {
            Destroy(obj.gameObject);
        }

        objects.Clear();
    }
}
