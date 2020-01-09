using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetIcosphere : MonoBehaviour
{
    [Range(1, 256)]
    public int resolution = 1;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, one }
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingFoldout;
    [HideInInspector]
    public bool colorSettingFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFaceTriangle[] terrainFaces;

    private void OnValidate()
    {
        GeneratePlanet();
    }
    
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[20];
        }
        terrainFaces = new TerrainFaceTriangle[20];


        for (int i = 0; i < 20; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFaceTriangle(shapeGenerator, meshFilters[i].sharedMesh, resolution);

            //DEBUG:
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }
    void GenerateMesh()
    {
        List<Vector3> icoVertices = new List<Vector3>();

        var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
        icoVertices.Add(new Vector3(-1, t, 0));
        icoVertices.Add(new Vector3(1, t, 0));
        icoVertices.Add(new Vector3(-1, -t, 0));
        icoVertices.Add(new Vector3(1, -t, 0));

        icoVertices.Add(new Vector3(0, -1, t));
        icoVertices.Add(new Vector3(0, 1, t));
        icoVertices.Add(new Vector3(0, -1, -t));
        icoVertices.Add(new Vector3(0, 1, -t));

        icoVertices.Add(new Vector3(t, 0, -1));
        icoVertices.Add(new Vector3(t, 0, 1));
        icoVertices.Add(new Vector3(-t, 0, -1));
        icoVertices.Add(new Vector3(-t, 0, 1));

        terrainFaces[0].ConstructMesh(icoVertices[0], icoVertices[11], icoVertices[5]);
        //DEBUG
        if (faceRenderMask == FaceRenderMask.All)
        {
            terrainFaces[1].ConstructMesh(icoVertices[0], icoVertices[5], icoVertices[1]);
            terrainFaces[2].ConstructMesh(icoVertices[0], icoVertices[1], icoVertices[7]);
            terrainFaces[3].ConstructMesh(icoVertices[0], icoVertices[7], icoVertices[10]);
            terrainFaces[4].ConstructMesh(icoVertices[0], icoVertices[10], icoVertices[11]);

            terrainFaces[5].ConstructMesh(icoVertices[1], icoVertices[5], icoVertices[9]);
            terrainFaces[6].ConstructMesh(icoVertices[5], icoVertices[11], icoVertices[4]);
            terrainFaces[7].ConstructMesh(icoVertices[11], icoVertices[10], icoVertices[2]);
            terrainFaces[8].ConstructMesh(icoVertices[10], icoVertices[7], icoVertices[6]);
            terrainFaces[9].ConstructMesh(icoVertices[7], icoVertices[1], icoVertices[8]);

            terrainFaces[10].ConstructMesh(icoVertices[3], icoVertices[9], icoVertices[4]);
            terrainFaces[11].ConstructMesh(icoVertices[3], icoVertices[4], icoVertices[2]);
            terrainFaces[12].ConstructMesh(icoVertices[3], icoVertices[2], icoVertices[6]);
            terrainFaces[13].ConstructMesh(icoVertices[3], icoVertices[6], icoVertices[8]);
            terrainFaces[14].ConstructMesh(icoVertices[3], icoVertices[8], icoVertices[9]);

            terrainFaces[15].ConstructMesh(icoVertices[4], icoVertices[9], icoVertices[5]);
            terrainFaces[16].ConstructMesh(icoVertices[2], icoVertices[4], icoVertices[11]);
            terrainFaces[17].ConstructMesh(icoVertices[6], icoVertices[2], icoVertices[10]);
            terrainFaces[18].ConstructMesh(icoVertices[8], icoVertices[6], icoVertices[7]);
            terrainFaces[19].ConstructMesh(icoVertices[9], icoVertices[8], icoVertices[1]);
        }

    }
    void GenerateColors()
    {
        foreach (var meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }
}
