using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private NetworkClient networkClient;

    [SerializeField] private ClickMarker clickMarker;
    [SerializeField] private Texture2D defaultCursor;

    public Vector2 CursorPixelOffset = new Vector2(6f, 6f);

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (!objectManager) objectManager = FindObjectOfType<ObjectManager>();
        playerCamera.MouseClick += OnMouseClick;
        playerCamera.MouseEnter += OnMouseEnter;
        playerCamera.MouseExit += OnMouseExit;


        SetCursorIcon(defaultCursor);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseExit(object sender, MouseClickEventArgs e)
    {
        SetCursorIcon(defaultCursor);
    }

    private void OnMouseEnter(object sender, MouseClickEventArgs e)
    {
        if (!networkClient || !networkClient.IsAuthenticated)
        {
            return;
        }

        var objId = -1;
        foreach (var hit in e.Collection)
        {
            if (int.TryParse(hit.collider.gameObject.name.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1], out objId))
            {
                break;
            }
        }

        var data = objectManager.GetObjectData(objId);
        if (!data)
        {
            return;
        }

        var defaultAction = data.Actions.FirstOrDefault();
        if (defaultAction)
        {
            SetCursorIcon(defaultAction.CursorIcon);
        }
    }

    private void SetCursorIcon(Texture2D cursorIcon)
    {
        Cursor.SetCursor(cursorIcon, CursorPixelOffset, CursorMode.Auto);
    }

    private void OnMouseClick(object sender, MouseClickEventArgs e)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        // depending on what is being clicked on
        // if its an object to interact with
        var worldObject = e.Collection.Select(x => x.collider.GetComponentInParent<NetworkObject>()).FirstOrDefault(x => x != null);
        //var worldObject = e.Object.transform.GetComponentInParent<NetworkObject>();
        if (worldObject)
        {
            myPlayer.MoveToAndInteractWith(ObjectInteraction.Create(worldObject), CheckIfRunning());
        }
        else
        {
            var hit = e.Collection.Select(x => new { terrain = x.collider.GetComponent<Terrain>(), point = x.point }).FirstOrDefault(x => x.terrain != null);
            //var terrain = e.Object.transform.GetComponent<Terrain>();
            if (hit != null && hit.terrain)
            {
                HandleTerrainInteraction(hit.terrain, hit.point);
                return;
            }

            myPlayer.MoveTo(hit.point, CheckIfRunning());
            DisplayMouseMarker(hit.point);
        }
    }
    private void HandleTerrainInteraction(Terrain terrain, Vector3 point)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        float groundHeight = terrain.SampleHeight(point);
        if (point.y - .033f > groundHeight)
        {
            HandleStaticObjectInteraction(terrain, point);
        }
        else
        {
            myPlayer.MoveTo(point, CheckIfRunning());
            DisplayMouseMarker(point);
        }
    }

    private void HandleStaticObjectInteraction(Terrain terrain, Vector3 point)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        int objInstance = -1;
        int objId = -1;
        int objCount = terrain.terrainData.treeInstances.Length;
        float objDist = 100;
        Vector3 objPos = new Vector3(0, 0, 0);

        // Notice we are looping through every terrain tree, 
        // which is a caution against a 15,000 tree terrain

        for (int i = 0; i < objCount; i++)
        {
            var tree = terrain.terrainData.treeInstances[i];
            Vector3 thisTreePos = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
            float thisTreeDist = Vector3.Distance(thisTreePos, point);

            if (thisTreeDist < objDist)
            {
                objId = tree.prototypeIndex;
                objInstance = i;
                objDist = thisTreeDist;
                objPos = thisTreePos;
            }
        }

        if (objDist > 10)
        {
            return;
        }

        myPlayer.MoveToAndInteractWith(ObjectInteraction.Create(new StaticObject
        {
            ObjectData = objectManager.GetObjectData(objId),
            ObjectId = objId,
            Instance = objInstance,
            Distance = objDist,
            Position = objPos,
        }), CheckIfRunning());
    }

    private void OnDestroy()
    {
        playerCamera.MouseClick -= OnMouseClick;
        playerCamera.MouseEnter -= OnMouseEnter;
        playerCamera.MouseExit -= OnMouseExit;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckIfRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisplayMouseMarker(Vector3 position)
    {
        if (!clickMarker) return;
        clickMarker.Show(position + (Vector3.up * 0.2f));
    }
}