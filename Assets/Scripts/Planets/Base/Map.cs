using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [ExecuteInEditMode]
    public class Map : MonoBehaviour
    {
        public bool debug;
        public int debugDivision;
        public bool useRandomSeed;

        private MapData mapData;
        public MapColorSettings colorSettings;
        public MapShapeSettings shapeSettings;

        private List<GameObject> triangleFaces;

        public void OnUpdateColorSettings()
        {
            UpdateMesh();
        }

        private void Start()
        {
            InitializeMap();
            GenerateMap();
            UpdateMesh();
        }

        void Update()
        {
            if (debug)
            {
                mapData.DebugDrawNodes(transform.position);
                mapData.DebugDrawNodesConnections(transform.position);
                //data.DebugDrawTriangles(transform.position);
            }
        }

        public void InitializeMap(bool forceUpdate = false)
        {
            if (triangleFaces == null)
            {
                triangleFaces = new List<GameObject>();
            }

            if (transform.childCount <= 0)
            {
                for (int i = 0; i < MapConfiguration.ICOSAHEDRON_FACES; i++)
                {
                    var obj = new GameObject();
                    obj.name = $"Face_{i}";
                    obj.AddComponent<MeshRenderer>();
                    obj.AddComponent<MeshFilter>();
                    obj.transform.SetParent(transform);

                    triangleFaces.Add(obj);
                }
            }
            else if (triangleFaces.Count == 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    triangleFaces.Add(child.gameObject);
                }
            }

            if (mapData == null || forceUpdate)
            {
                mapData = new MapData();
                mapData.InitMapData(debugDivision);
            }
        }
        public void GenerateMap()
        {
            if (useRandomSeed)
            {
                shapeSettings.seed = Random.Range(int.MinValue, int.MaxValue);
            }

            mapData.GenerateHeights(shapeSettings);
        }
        public void UpdateMesh()
        {
            var meshes = mapData.GetMeshes();
            for(int i = 0; i < meshes.Length; i++)
            {
                var filter = triangleFaces[i].GetComponent<MeshFilter>();
                var render = triangleFaces[i].GetComponent<MeshRenderer>();

                filter.mesh = meshes[i];
                render.material = colorSettings.material;
            }
        }
    }
}

