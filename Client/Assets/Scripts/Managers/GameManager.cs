using Assets.Scripts.Extensions;
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
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ClickMarker clickMarker;
    [SerializeField] private Texture2D defaultCursor;

    public Vector2 CursorPixelOffset = new Vector2(6f, 6f);

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
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
        if (!playerManager.Me) return;

        var myPlayer = playerManager.Me;

        if (e.MouseButton == MouseButton.Right)
        {
            HandleRightClick(myPlayer, e);
            return;
        }

        HandleLeftClick(myPlayer, e);
    }

    private void HandleLeftClick(NetworkPlayer me, MouseClickEventArgs e)
    {
        uiManager.ContextMenu.Hide();

        // ignore other players for now since we don't have
        // a left button action on players yet. Going to be for selecting/targeting players.
        //var player = e.GetNetworkPlayer();
        //if (player)
        //{
        //    return;
        //}

        var worldObject = e.GetNetworkObject();
        //var worldObject = e.Object.transform.GetComponentInParent<NetworkObject>();
        if (worldObject)
        {
            me.MoveToAndInteractWith(ObjectInteraction.Create(worldObject), CheckIfRunning());
            return;
        }

        var terrainHit = e.GetTerrain();
        //var terrain = e.Object.transform.GetComponent<Terrain>();
        if (terrainHit.Terrain)
        {
            HandleTerrainInteraction(e, terrainHit.Terrain, terrainHit.Point);
            return;
        }

        WalkTo(me, terrainHit.Point);
    }

    private void HandleRightClick(NetworkPlayer me, MouseClickEventArgs e)
    {
        var player = e.GetNetworkPlayer();
        if (player)
        {
            return;
        }

        var worldObject = e.GetNetworkObject();
        //var worldObject = e.Object.transform.GetComponentInParent<NetworkObject>();
        if (worldObject)
        {
            uiManager.ContextMenu
                .SetHeader(worldObject.ObjectData.Name)
                .Show();
            return;
        }

        var terrainHit = e.GetTerrain();
        //var terrain = e.Object.transform.GetComponent<Terrain>();
        if (terrainHit.Terrain)
        {
            var staticObjectHit = e.Collection.FirstOrDefault(x => x.collider.name.Contains("::"));
            if (staticObjectHit.point != Vector3.zero || IsStaticObject(terrainHit.Terrain, terrainHit.Point))
            {
                var objCollider = e.GetStaticCollider();
                var objId = objCollider.gameObject.GetObjectId();
                if (objId >= 0)
                {
                    var data = objectManager.GetObjectData(objId);
                    var actions = data.Actions.Select(x =>
                         new ContextMenuItem
                         {
                             Text = x.Name,
                             Click = () => HandleStaticObjectInteraction(terrainHit.Terrain, terrainHit.Point, x.Id, objCollider),
                             ActionId = x.Id
                         }
                    );

                    uiManager.ContextMenu
                      .SetHeader(data.Name)
                      .SetItems(actions.ToArray())
                      .Show();
                }
            }
            else
            {
                uiManager.ContextMenu
                    .SetHeader("Terrain")
                    .SetItems(new ContextMenuItem
                    {
                        Text = "Walk Here",
                        Click = () => WalkTo(me, terrainHit.Point)
                    })
                    .Show();
            }
            return;
        }
    }

    private void HandleTerrainInteraction(MouseClickEventArgs e, Terrain terrain, Vector3 point)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        var staticObjectHit = e.Collection.FirstOrDefault(x => x.collider.name.Contains("::"));
        if (staticObjectHit.point != Vector3.zero || IsStaticObject(terrain, point))
        {
            HandleStaticObjectInteraction(terrain, point);
        }
        else
        {
            WalkTo(myPlayer, point);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsStaticObject(Terrain terrain, Vector3 point)
    {
        float groundHeight = terrain.SampleHeight(point);
        return point.y - .033f > groundHeight;
    }

    private void HandleStaticObjectInteraction(Terrain terrain, Vector3 point, int actionId = -1, Collider objectCollider = null)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        int objInstance = -1;
        int objId = -1;
        int objCount = terrain.terrainData.treeInstances.Length;
        float objDist = 100;
        Vector3 objPos = new Vector3(0, 0, 0);

        if (objectCollider)
        {
            objId = objectCollider.gameObject.GetObjectId();
            objInstance = objectCollider.gameObject.GetObjectIndex();
            objDist = Vector3.Distance(objectCollider.transform.position, point);
            objPos = objectCollider.transform.position;
        }
        else
        {
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
        }

        InteractWithStaticObject(actionId, myPlayer, objInstance, objId, objDist, objPos);
    }

    private void InteractWithStaticObject(int actionId, NetworkPlayer myPlayer, int objInstance, int objId, float objDist, Vector3 objPos)
    {
        myPlayer.MoveToAndInteractWith(ObjectInteraction.Create(new StaticObject
        {
            ObjectData = objectManager.GetObjectData(objId),
            ObjectId = objId,
            Instance = objInstance,
            Distance = objDist,
            Position = objPos,
        }, actionId), CheckIfRunning());
    }

    private void WalkTo(NetworkPlayer me, Vector3 position)
    {
        me.MoveTo(position, CheckIfRunning());
        DisplayMouseMarker(position);
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