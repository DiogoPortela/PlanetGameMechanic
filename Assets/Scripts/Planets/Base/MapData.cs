using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    internal class MapData
    {
        private MapNode[] allNodes;
        private MapTriangle[] allTriangles;
        private MapTriangle[] trianglesHead;

        private Mesh[] meshes;

        public bool InitMapData(int divisions)
        {
            MapGenerator.GenerateIcosahedron(out allNodes, out allTriangles);

            trianglesHead = (MapTriangle[])allTriangles.Clone();

            for (int i = 0; i < divisions; i++)
            {
                MapGenerator.Subdivide(ref allNodes, ref allTriangles);
            }
            return true;
        }
        public bool GenerateHeights(MapShapeSettings shapeSettings)
        {
            MapGenerator.GenerateHeights(ref allNodes, shapeSettings);
            return true;
        }
        public Mesh[] GetMeshes(bool forceUpdate = false)
        {
            if(meshes == null || forceUpdate)
            {
                meshes = MapGenerator.GenerateMeshes(ref trianglesHead);
            }

            return meshes;
        }

        public MapTriangle FindTriangleForPosition(Vector3 vector)
        {
            MapTriangle result = null;
            foreach (var triangle in trianglesHead)
            {
                if (triangle.IsLineInsideTriangle(vector))
                {
                    result = triangle.GetTriangleForPosition(vector);
                    break;
                }
                else
                {
                    continue;
                }
            }

            return result;
        }

        public void DebugDrawNodes(Vector3 origin)
        {
            foreach (var node in allNodes)
            {
                node.DrawDebugLine(origin);
            }
        }
        public void DebugDrawNodesConnections(Vector3 origin)
        {
            foreach (var node in allNodes)
            {
                node.DrawDebugConnections(origin);
            }
        }
        public void DebugDrawTriangles(Vector3 origin)
        {
            foreach (var tri in allTriangles)
            {
                tri.DebugDrawTriangle(origin);
            }
        }
    }
}

