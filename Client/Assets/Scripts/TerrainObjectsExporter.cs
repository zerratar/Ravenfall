using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TerrainObjectsExporter : MonoBehaviour
{
    private Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        this.terrain = this.GetComponent<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StringBuilder sb = new StringBuilder();
            var trees = new List<SceneObject>();
            for (var treeIndex = 0; treeIndex < terrain.terrainData.treeInstanceCount; ++treeIndex)
            {
                var tree = terrain.terrainData.GetTreeInstance(treeIndex);

                var pos = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
                //var x = pos.x.ToString("0.000").Replace(",", ".");
                //var y = pos.y.ToString("0.000").Replace(",", ".");
                //var z = pos.z.ToString("0.000").Replace(",", ".");

                var obj = new SceneObject
                {
                    ObjectId = tree.prototypeIndex,
                    DisplayObjectId = tree.prototypeIndex,
                    Position = pos,
                    Experience = 15,
                    InteractItemType = 1,
                    RespawnMilliseconds = 5000,
                    Static = true
                };
                trees.Add(obj);
                //sb.AppendLine($"entities.Add(TreeObject.Create(ref index, new Vector3({x}f, {y}f, {z}f)));");

            }

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(trees);
            //var data = JsonUtility.ToJson(trees);
            System.IO.File.WriteAllText("C:\\git\\ravenfall\\Server\\src\\Servers\\GameServer\\bin\\Debug\\netcoreapp3.1\\repositories\\Shinobytes.Ravenfall.RavenNet.Models.SceneObject.json", data.ToString());
        }
    }



    [System.Serializable]
    public class SceneObject
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int DisplayObjectId { get; set; }
        public Shinobytes.Ravenfall.RavenNet.Models.Vector3 Position { get; set; }
        public decimal Experience { get; set; }
        public int InteractItemType { get; set; }
        public int RespawnMilliseconds { get; set; }
        public bool Static { get; set; }
    }
}
