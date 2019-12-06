using System.Collections;
using System.Collections.Generic;
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


    public void ConstructMesh(Vector3 vert1, Vector3 vert2, Vector3 vert3)
    {
        vert1 = shapeGenerator.CalculatePointOnPlanet(vert1);
        vert2 = shapeGenerator.CalculatePointOnPlanet(vert2);
        vert3 = shapeGenerator.CalculatePointOnPlanet(vert3);
        List<Vector3> vertices = new List<Vector3> { vert1, vert2, vert3 };
        List<Vector3Int> triangles = new List<Vector3Int> { new Vector3Int(0, 1, 2) };  //initial triangle

        //Calculate Normal
        var side1 = vert1 - vert2;
        var side2 = vert1 - vert3;
        localUp = Vector3.Cross(side1, side2);

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        // Divide the triangles
        for (int i = 1; i < resolution; i++)
        {
            List<Vector3Int> trianglesAux = new List<Vector3Int>();
            foreach (var tri in triangles)
            {
                var vertXY = FindMiddlePoint(vertices[tri.x], vertices[tri.y]);
                vertXY = shapeGenerator.CalculatePointOnPlanet(vertXY);
                var vertXZ = FindMiddlePoint(vertices[tri.x], vertices[tri.z]);
                vertXZ = shapeGenerator.CalculatePointOnPlanet(vertXZ);
                var vertYZ = FindMiddlePoint(vertices[tri.y], vertices[tri.z]);
                vertYZ = shapeGenerator.CalculatePointOnPlanet(vertYZ);

                if (!vertices.Contains(vertXY))
                    vertices.Add(vertXY);
                if (!vertices.Contains(vertXZ))
                    vertices.Add(vertXZ);
                if (!vertices.Contains(vertYZ))
                    vertices.Add(vertYZ);

                var indexXY = vertices.IndexOf(vertXY);
                var indexXZ = vertices.IndexOf(vertXZ);
                var indexYZ = vertices.IndexOf(vertYZ);

                trianglesAux.Add(new Vector3Int(tri.x, indexXY, indexXZ));
                trianglesAux.Add(new Vector3Int(tri.y, indexYZ, indexXY));
                trianglesAux.Add(new Vector3Int(tri.z, indexXZ, indexYZ));
                trianglesAux.Add(new Vector3Int(indexXY, indexYZ, indexXZ));
            }
            triangles = trianglesAux;
        }

        var finalVertices = vertices.ToArray();
        int[] finalTriangles = new int[(int)Mathf.Pow(4, (resolution - 1)) * 3];
        //int[] finalTriangles = new int[((resolution - 2) * (resolution - 2) + 2 * (resolution - 2) + 1) * 3];

        int j = 0;
        foreach (var tri in triangles)
        {
            finalTriangles[j] = tri.x;
            finalTriangles[j + 1] = tri.y;
            finalTriangles[j + 2] = tri.z;
            j += 3;
        }

        mesh.Clear();
        mesh.vertices = finalVertices;
        mesh.triangles = finalTriangles;
        mesh.RecalculateNormals();
    }

    private Vector3 FindMiddlePoint(Vector3 vert1, Vector3 vert2)
    {
        return new Vector3(
            (vert1.x + vert2.x) / 2.0f,
            (vert1.y + vert2.y) / 2.0f,
            (vert1.z + vert2.z) / 2.0f);
    }
}
