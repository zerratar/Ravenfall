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
    [SerializeField] private Transform staticObjectColliderContainer;


    private Terrain activeTerrain;
    private TreeInstance[] originalStaticObjectData;
    private readonly List<NetworkObject> objects = new List<NetworkObject>();
    private readonly List<CapsuleCollider> staticObjectColliders = new List<CapsuleCollider>();

    void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        spawnableObjects = Resources.LoadAll<ServerObject>("Data/Objects");
        LoadStaticObjectColliders();
    }

    private void OnApplicationExit()
    {
        CleanupStaticObjects();
    }

    private void CleanupStaticObjects()
    {
        activeTerrain.terrainData.treeInstances = new TreeInstance[0];
        for (int i = 0; i < originalStaticObjectData.Length; i++)
        {
            activeTerrain.AddTreeInstance(originalStaticObjectData[i]);
        }
        activeTerrain.Flush();
    }

    internal void OnObjectAdded(SceneObject entity)
    {
        var networkObject = Instantiate(objectPrefab, objectContainer).GetComponent<NetworkObject>();
        networkObject.Data = GetObjectData(entity);
        networkObject.ServerId = entity.Id;
        networkObject.name = networkObject.Data.Name;
        networkObject.transform.position = entity.Position;

        Instantiate(networkObject.Data.Model, networkObject.transform);
        objects.Add(networkObject);
    }

    internal void OnObjectUpdated(SceneObject entity)
    {
        Debug.Log("OnObjectUpdated");
        var networkObject = objects.FirstOrDefault(x => x.ServerId == entity.Id);
        if (!networkObject)
        {
            if (entity.Static)
            {
                UpdateStaticObject(entity);
            }
            return;
        }

        var oldObjectId = networkObject.Data.Id;

        networkObject.Data = GetObjectData(entity);
        networkObject.name = networkObject.Data.Name;
        networkObject.transform.position = entity.Position;

        if (entity.ObjectId != oldObjectId)
        {
            var model = networkObject.transform.GetChild(0);
            Destroy(model.gameObject);
            Instantiate(networkObject.Data.Model, networkObject.transform);
        }
    }

    private void LoadStaticObjectColliders()
    {
        if (!staticObjectColliderContainer) return;
        if (!activeTerrain)
            activeTerrain = FindObjectOfType<Terrain>();

        int objCount = activeTerrain.terrainData.treeInstances.Length;
        var trees = activeTerrain.terrainData.treeInstances;

        originalStaticObjectData = activeTerrain.terrainData.treeInstances;

        for (int i = 0; i < objCount; i++)
        {
            var tree = trees[i];
            var thisTreePos = UnityEngine.Vector3.Scale(tree.position, activeTerrain.terrainData.size) + activeTerrain.transform.position;
            var prototype = activeTerrain.terrainData.treePrototypes[tree.prototypeIndex];

            var go = new GameObject("ObjectCollider::" + tree.prototypeIndex + "::" + i);
            go.transform.SetParent(staticObjectColliderContainer);
            go.transform.position = thisTreePos;

            var collider = prototype.prefab.GetComponent<CapsuleCollider>();
            if (collider)
            {
                var bc = go.AddComponent<CapsuleCollider>();
                bc.center = collider.center;
                bc.radius = collider.radius;
                bc.height = collider.height;
                bc.direction = collider.direction;
                staticObjectColliders.Add(bc);
            }
        }
    }

    private void UpdateStaticObject(SceneObject entity)
    {
        if (!activeTerrain)
            activeTerrain = FindObjectOfType<Terrain>();

        int objCount = activeTerrain.terrainData.treeInstances.Length;
        // Notice we are looping through every terrain tree, 
        // which is a caution against a 15,000 tree terrain

        var oldTrees = activeTerrain.terrainData.treeInstances;
        activeTerrain.terrainData.treeInstances = new TreeInstance[0];
        for (int i = 0; i < objCount; i++)
        {
            var tree = oldTrees[i];
            var thisTreePos = UnityEngine.Vector3.Scale(tree.position, activeTerrain.terrainData.size) + activeTerrain.transform.position;
            var thisTreeDist = UnityEngine.Vector3.Distance(thisTreePos, entity.Position);

            if (thisTreeDist < 0.2f)
            {
                tree.prototypeIndex = entity.ObjectId;

                var objCollider = staticObjectColliders[i];
                if (objCollider)
                {
                    var prototype = activeTerrain.terrainData.treePrototypes[tree.prototypeIndex];
                    objCollider.gameObject.name = "ObjectCollider::" + tree.prototypeIndex + "::" + i;
                    var collider = prototype.prefab.GetComponent<CapsuleCollider>();
                    if (collider)
                    {
                        objCollider.center = collider.center;
                        objCollider.radius = collider.radius;
                        objCollider.height = collider.height;
                        objCollider.direction = collider.direction;
                    }
                }
            }

            activeTerrain.AddTreeInstance(tree);
        }
        //activeTerrain.Flush();
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
    public ServerObject GetObjectData(SceneObject entity)
    {
        return spawnableObjects.FirstOrDefault(x => x.Id == entity.ObjectId);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ServerObject GetObjectData(int id)
    {
        return spawnableObjects.FirstOrDefault(x => x.Id == id);
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
