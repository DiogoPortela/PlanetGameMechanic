using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainFaceTriangle
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFaceTriangle(ShapeGenerator shapeGenerator, Mesh mesh, int resolution)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
    }

    /// <summary>
    /// Calculates all vertices and triangle indexes from a starting triangle. Does this dividing the triangle by the resolution value, and then calculates each triangle for each frow. Use the parameters to define the starting triangle.
    /// </summary>   
    public void ConstructMesh(Vector3 vert1, Vector3 vert2, Vector3 vert3)
    {
        int vertexIndex = 0;
        Dictionary<Vector3, int> vertices = new Dictionary<Vector3, int> { { vert1, vertexIndex++ } };
        List<Vector3Int> triangles = new List<Vector3Int>();

        int triangleCountAux = 1;   //number of vertex on the row.
        int vertexCountAux = 1;     //number of vertex on the lower limit of the row - 1.

        List<Vector3> topRowVectices = new List<Vector3> { vert1 };
        List<Vector3> bottomRowVertices = new List<Vector3>();

        for (int row = 1; row <= resolution; row++)
        {
            Vector3 leftVert = FindPerCentPoint(vert1, vert3, 1.0f / resolution * row);
            Vector3 rightVert = FindPerCentPoint(vert1, vert2, 1.0f / resolution * row);

            for (int i = 0; i <= vertexCountAux; i++)
            {
                bottomRowVertices.Add(FindPerCentPoint(leftVert, rightVert, ((float)i / (float)vertexCountAux)));
            }
            foreach (var vertex in bottomRowVertices)
            {
                vertices.Add(vertex, vertexIndex++);
            }

            int pointingDownVertexCount = (int)(triangleCountAux / 2.0f);
            int pointingUpVertexCount = pointingDownVertexCount + 1;

            for (int upVertex = 0; upVertex < pointingUpVertexCount; upVertex++)
            {
                triangles.Add(new Vector3Int(vertices[topRowVectices[upVertex]], vertices[bottomRowVertices[upVertex + 1]], vertices[bottomRowVertices[upVertex]]));
            }
            for (int downVertex = 0; downVertex < pointingDownVertexCount; downVertex++)
            {
                triangles.Add(new Vector3Int(vertices[topRowVectices[downVertex]], vertices[topRowVectices[downVertex + 1]], vertices[bottomRowVertices[downVertex + 1]]));
            }

            topRowVectices = bottomRowVertices;
            bottomRowVertices = new List<Vector3>();
            triangleCountAux += 2;
            vertexCountAux++;
        }

        var finalVertices = vertices.Keys.ToArray();
        int[] finalTriangles = new int[resolution * resolution * 3];

        int triIndex = 0;
        foreach (var tri in triangles)
        {
            finalTriangles[triIndex] = tri.x;
            finalTriangles[triIndex + 1] = tri.y;
            finalTriangles[triIndex + 2] = tri.z;
            triIndex += 3;
        }

        for (int i = 0; i < finalVertices.Length; i++)
        {
            //finalVertices[i] = shapeGenerator.CalculatePointOnPlanet(finalVertices[i]);
            finalVertices[i] = shapeGenerator.GetPointOnPlanet(finalVertices[i]);
        }

        mesh.Clear();
        mesh.vertices = finalVertices;
        mesh.triangles = finalTriangles;
        mesh.RecalculateNormals();
    }

    private Vector3 FindPerCentPoint(Vector3 vert1, Vector3 vert2, float percent)
    {
        Vector3 dir = (vert2 - vert1) * percent;
        return vert1 + dir;
    }
}
