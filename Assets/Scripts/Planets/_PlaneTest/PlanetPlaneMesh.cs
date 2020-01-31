using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlaneMesh : MonoBehaviour
{
    [Range(2, 256)]
    public int meshResolution = 10;
    public int hemisphereDistorcionPower = 4;
    [Range(0, 10)]
    public float zValue = 0.0f;
    public ShapeSettings shapeSettings;

    [HideInInspector]
    public bool shapeSettingFoldout;

    ShapeGenerator shapeGenerator;
    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] originalMeshShape;

    private void OnValidate()
    {
        Init();
    }

    public void Init()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        GenerateMesh();
        ProjectOntoPlanet();
    }

    private void GenerateMesh()
    {
        if (meshFilter == null)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
            this.gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }
        if (mesh == null)
        {
            mesh = meshFilter.sharedMesh;
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        //Generate the mesh
        for (int x = 0; x < meshResolution; x++)
            for (int y = 0; y < meshResolution; y++)
                vertices.Add(new Vector3((float)x / (meshResolution - 1) - 0.5f, (float)y / (meshResolution - 1) -0.5f));

        for (int x = 0; x < meshResolution - 1; x++)
        {
            for (int y = 0; y < meshResolution - 1; y++)
            {
                int currentIndex = y * meshResolution + x;
                triangles.Add(currentIndex);
                triangles.Add(currentIndex + meshResolution);
                triangles.Add(currentIndex + 1);

                triangles.Add(currentIndex + 1);
                triangles.Add(currentIndex + meshResolution);
                triangles.Add(currentIndex + meshResolution + 1);
            }
        }

        //Deform the mesh into a hemisphere
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y, (1 - Mathf.Pow(vertices[i].x, hemisphereDistorcionPower)) * (1 - Mathf.Pow(vertices[i].y, hemisphereDistorcionPower)) - 0.75f);
        }

        mesh.Clear();
        originalMeshShape = vertices.ToArray();
        mesh.vertices = originalMeshShape;
        mesh.triangles = triangles.ToArray();

    }
    private void ProjectOntoPlanet()
    {
        var vertices = originalMeshShape;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = (vertices[i] + Vector3.forward * zValue).normalized;
            vertices[i] = shapeGenerator.CalculatePointOnPlanet(vertices[i]);
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    public void OnShapeSettingsUpdated()
    {
        Init();
    }
}
