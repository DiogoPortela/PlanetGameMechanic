using pt.dportela.PlanetGame.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    internal static class MapGenerator
    {
        // Generate a graph itself.
        public static void GenerateIcosahedron(out MapNode[] vertices, out MapTriangle[] triangles)
        {
            vertices = new MapNode[MapConfiguration.ICOSAHEDRON_VERTICES];

            var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
            vertices[0] = new MapNode(new Vector3(-1, t, 0).normalized);
            vertices[1] = new MapNode(new Vector3(1, t, 0).normalized);
            vertices[2] = new MapNode(new Vector3(-1, -t, 0).normalized);
            vertices[3] = new MapNode(new Vector3(1, -t, 0).normalized);

            vertices[4] = new MapNode(new Vector3(0, -1, t).normalized);
            vertices[5] = new MapNode(new Vector3(0, 1, t).normalized);
            vertices[6] = new MapNode(new Vector3(0, -1, -t).normalized);
            vertices[7] = new MapNode(new Vector3(0, 1, -t).normalized);

            vertices[8] = new MapNode(new Vector3(t, 0, -1).normalized);
            vertices[9] = new MapNode(new Vector3(t, 0, 1).normalized);
            vertices[10] = new MapNode(new Vector3(-t, 0, -1).normalized);
            vertices[11] = new MapNode(new Vector3(-t, 0, 1).normalized);

            vertices[0].AddNeighbour(ref vertices[1]);
            vertices[0].AddNeighbour(ref vertices[5]);
            vertices[0].AddNeighbour(ref vertices[11]);
            vertices[0].AddNeighbour(ref vertices[10]);
            vertices[0].AddNeighbour(ref vertices[7]);

            vertices[3].AddNeighbour(ref vertices[8]);
            vertices[3].AddNeighbour(ref vertices[9]);
            vertices[3].AddNeighbour(ref vertices[4]);
            vertices[3].AddNeighbour(ref vertices[2]);
            vertices[3].AddNeighbour(ref vertices[6]);

            vertices[8].AddNeighbour(ref vertices[7]);
            vertices[8].AddNeighbour(ref vertices[1]);
            vertices[7].AddNeighbour(ref vertices[1]);

            vertices[9].AddNeighbour(ref vertices[1]);
            vertices[9].AddNeighbour(ref vertices[5]);
            vertices[5].AddNeighbour(ref vertices[1]);

            vertices[9].AddNeighbour(ref vertices[8]);

            vertices[4].AddNeighbour(ref vertices[11]);
            vertices[4].AddNeighbour(ref vertices[5]);
            vertices[11].AddNeighbour(ref vertices[5]);

            vertices[9].AddNeighbour(ref vertices[4]);

            vertices[2].AddNeighbour(ref vertices[11]);
            vertices[2].AddNeighbour(ref vertices[10]);
            vertices[11].AddNeighbour(ref vertices[10]);

            vertices[4].AddNeighbour(ref vertices[2]);

            vertices[6].AddNeighbour(ref vertices[7]);
            vertices[6].AddNeighbour(ref vertices[10]);
            vertices[7].AddNeighbour(ref vertices[10]);

            vertices[8].AddNeighbour(ref vertices[6]);
            vertices[6].AddNeighbour(ref vertices[2]);

            // Generating the triangles:
            triangles = new MapTriangle[MapConfiguration.ICOSAHEDRON_FACES];

            triangles[0] = new MapTriangle(vertices[0], vertices[1], vertices[7]);
            triangles[1] = new MapTriangle(vertices[1], vertices[0], vertices[5]);
            triangles[2] = new MapTriangle(vertices[7], vertices[10], vertices[0]);
            triangles[3] = new MapTriangle(vertices[0], vertices[10], vertices[11]);
            triangles[4] = new MapTriangle(vertices[0], vertices[11], vertices[5]);

            triangles[5] = new MapTriangle(vertices[6], vertices[8], vertices[3]);
            triangles[6] = new MapTriangle(vertices[3], vertices[8], vertices[9]);
            triangles[7] = new MapTriangle(vertices[3], vertices[9], vertices[4]);
            triangles[8] = new MapTriangle(vertices[3], vertices[4], vertices[2]);
            triangles[9] = new MapTriangle(vertices[3], vertices[2], vertices[6]);

            triangles[10] = new MapTriangle(vertices[7], vertices[8], vertices[6]);
            triangles[11] = new MapTriangle(vertices[8], vertices[7], vertices[1]);
            triangles[12] = new MapTriangle(vertices[8], vertices[1], vertices[9]);
            triangles[13] = new MapTriangle(vertices[9], vertices[1], vertices[5]);
            triangles[14] = new MapTriangle(vertices[4], vertices[9], vertices[5]);
            triangles[15] = new MapTriangle(vertices[11], vertices[4], vertices[5]);
            triangles[16] = new MapTriangle(vertices[4], vertices[11], vertices[2]);
            triangles[17] = new MapTriangle(vertices[10], vertices[2], vertices[11]);
            triangles[18] = new MapTriangle(vertices[2], vertices[10], vertices[6]);
            triangles[19] = new MapTriangle(vertices[10], vertices[7], vertices[6]);

            Debug.Log($"MapGenerator : Generated Icosahedron with {MapConfiguration.ICOSAHEDRON_FACES} triangles and {MapConfiguration.ICOSAHEDRON_VERTICES} nodes.");

        }
        public static void GenerateDebugIcosahedron(out MapNode[] vertices, out MapTriangle[] triangle)
        {
            vertices = new MapNode[3];
            var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
            vertices[0] = new MapNode(new Vector3(-1, t, 0).normalized);
            vertices[1] = new MapNode(new Vector3(1, t, 0).normalized);
            vertices[2] = new MapNode(new Vector3(0, 1, -t).normalized);

            vertices[0].AddNeighbour(ref vertices[1]);
            vertices[1].AddNeighbour(ref vertices[2]);
            vertices[2].AddNeighbour(ref vertices[0]);

            triangle = new MapTriangle[1];
            triangle[0] = new MapTriangle(vertices[0], vertices[1], vertices[2]);

            Debug.Log("MapGenerator : Generated Debug Icosahedron");

        }
        public static bool Subdivide(ref MapNode[] allNodes, ref MapTriangle[] allTriangles)
        {
            List<MapTriangle> resultingTriangles = new List<MapTriangle>();
            List<MapNode> resultingNodes = new List<MapNode>(allNodes);

            foreach (var triangle in allTriangles)
            {
                var triangleNodes = triangle.GetVertices();
                MapNode[] newNodes = new MapNode[3];

                newNodes[0] = GetNodeBetweenNodes(triangleNodes[0], triangleNodes[1], ref resultingNodes);
                newNodes[1] = GetNodeBetweenNodes(triangleNodes[1], triangleNodes[2], ref resultingNodes);
                newNodes[2] = GetNodeBetweenNodes(triangleNodes[2], triangleNodes[0], ref resultingNodes);

                newNodes[0].AddNeighbour(ref newNodes[1]);
                newNodes[1].AddNeighbour(ref newNodes[2]);
                newNodes[2].AddNeighbour(ref newNodes[0]);

                MapTriangle[] newTriangles = new MapTriangle[4];

                newTriangles[0] = new MapTriangle(triangleNodes[0], newNodes[0], newNodes[2], triangle);
                newTriangles[1] = new MapTriangle(newNodes[0], triangleNodes[1], newNodes[1], triangle);
                newTriangles[2] = new MapTriangle(newNodes[0], newNodes[1], newNodes[2], triangle);
                newTriangles[3] = new MapTriangle(newNodes[2], newNodes[1], triangleNodes[2], triangle);

                resultingTriangles.AddRange(newTriangles);
            }


            allTriangles = resultingTriangles.ToArray();
            allNodes = resultingNodes.ToArray();

            Debug.Log($"MapGenerator: Subdivided into {resultingTriangles.Count} triangles and {resultingNodes.Count} nodes.");

            return true;
        }
        private static MapNode GetNodeBetweenNodes(MapNode a, MapNode b, ref List<MapNode> allNodes)
        {
            var position = GetPositionBetweenNodes(a, b);

            foreach (var node in allNodes)
            {
                if (node.position == position)
                {
                    return node;
                }
            }

            var newNode = new MapNode(position);
            allNodes.Add(newNode);

            if (a.IsNeighbourWith(b))
            {
                a.RemoveNeighbour(ref b);
                newNode.AddNeighbour(ref a);
                newNode.AddNeighbour(ref b);
            }

            return newNode;
        }
        private static Vector3 GetPositionBetweenNodes(MapNode a, MapNode b)
        {
            return (a.position + (b.position - a.position) / 2.0f).normalized;
        }


        // Populate a graph with data.
        public static bool GenerateHeights(ref MapNode[] vertices, in MapShapeSettings shapeSettings)
        {
            var noise = new Noise(shapeSettings.seed);
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i].height = shapeSettings.planetBaseRadius + (noise.Evaluate(vertices[i].position) + 1) / 2.0f * shapeSettings.mountainMaxHeight;
            }

            return true;
        }


        // Generate a mesh from a graph.
        public static Mesh[] GenerateMeshes(ref MapTriangle[] triangles)
        {
            var meshes = new Mesh[MapConfiguration.ICOSAHEDRON_FACES];

            for (int i = 0; i < triangles.Length; i++)
            {
                MapTriangle triangle = triangles[i];
                var children = triangle.GetChildren();

                var verticesDictionary = new Dictionary<Vector3, MapNode>();
                foreach (var child in children)
                {
                    var vertices = child.GetVertices();
                    for (int verticeIndex = 0; verticeIndex < vertices.Length; verticeIndex++)
                    {
                        var vertice = vertices[verticeIndex];
                        if (verticesDictionary.ContainsKey(vertice.GetFinalPosition()))
                        {
                            continue;
                        }
                        else
                        {
                            verticesDictionary.Add(vertice.GetFinalPosition(), vertice);
                        }
                    }
                }
                var resultingVertices = verticesDictionary.Keys.ToList();

                var triangleIndex = new List<int>();
                foreach (var child in children)
                {
                    var vertices = child.GetVertices();

                    triangleIndex.Add(resultingVertices.IndexOf(vertices[0].GetFinalPosition()));
                    triangleIndex.Add(resultingVertices.IndexOf(vertices[1].GetFinalPosition()));
                    triangleIndex.Add(resultingVertices.IndexOf(vertices[2].GetFinalPosition()));
                }

                var resultingMesh = new Mesh();
                resultingMesh.vertices = resultingVertices.ToArray();
                resultingMesh.triangles = triangleIndex.ToArray();
                resultingMesh.RecalculateNormals();

                meshes[i] = resultingMesh;
            }

            return meshes;
        }
        
    }
}
