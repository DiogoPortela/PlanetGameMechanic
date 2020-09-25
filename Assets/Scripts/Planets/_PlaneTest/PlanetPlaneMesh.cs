using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    public class PlanetPlaneMesh : MonoBehaviour
    {
        [Range(2, 256)]
        public int meshResolution = 10;
        public int hemisphereDistorcionPower = 4;
        [Range(0, 10)]
        public float zValue = 0.0f;
        public float zMin = 0.0f;
        public float zMax = 1.0f;
        public ShapeSettings shapeSettings;

        [HideInInspector]
        public bool shapeSettingFoldout;

        ShapeGenerator shapeGenerator;
        MeshFilter meshFilter;
        Mesh mesh;
        Vector3[] originalMeshShape;
        Vector3 currentForward;

        private void OnValidate()
        {
            //GeneratePlanet();
        }
        private void Start()
        {
            GeneratePlanet();
        }
        public void Update()
        {
            RecalculateMesh();
        }

        // Editor functions:
        public void OnShapeSettingsUpdated()
        {
            GeneratePlanet();
        }


        // Generation Code:
        public void GeneratePlanet()
        {
            Init();
            GenerateMesh();
            RecalculateMesh();
        }

        private void Init()
        {
            shapeGenerator = new ShapeGenerator(shapeSettings);

            if (meshFilter == null)
            {
                meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter == null)
                    meshFilter = this.gameObject.AddComponent<MeshFilter>();

                if (gameObject.GetComponent<MeshRenderer>() == null)
                    this.gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

                mesh = new Mesh();
                meshFilter.sharedMesh = mesh;
            }
            if (mesh == null)
            {
                mesh = meshFilter.sharedMesh;
            }
        }
        private void GenerateMesh()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            //Generate the mesh
            for (int x = 0; x < meshResolution; x++)
                for (int y = 0; y < meshResolution; y++)
                    vertices.Add(new Vector3((float)x / (meshResolution - 1) - 0.5f, (float)y / (meshResolution - 1) - 0.5f));

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
                vertices[i] = new Vector3(vertices[i].x, vertices[i].y, (1 - Mathf.Pow(vertices[i].x, hemisphereDistorcionPower)) * (1 - Mathf.Pow(vertices[i].y, hemisphereDistorcionPower)));
            }

            currentForward = Vector3.forward;

            mesh.Clear();
            originalMeshShape = vertices.ToArray();
            mesh.vertices = originalMeshShape;
            mesh.triangles = triangles.ToArray();

        }
        public void RecalculateMesh()
        {
            var vertices = (Vector3[])originalMeshShape.Clone();
            OffsetZ(ref vertices);
            LookAtCamera(ref vertices);
            ProjectOntoPlanetSurface(ref vertices);

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
        }
        private void OffsetZ(ref Vector3[] vertices)
        {
            var distanceCameraToPlane = (Camera.main.transform.position - transform.position).magnitude;
            if (distanceCameraToPlane > zMax)
            {
                zValue = 0;
            }
            else if (distanceCameraToPlane < zMin)
            {
                zValue = 1;
            }
            else
            {
                zValue = -distanceCameraToPlane + zMax;

            }
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector3(0, 0, zValue - 0.75f);
            }
        }
        private void LookAtCamera(ref Vector3[] vertices)
        {
            currentForward = (Camera.main.transform.position - this.transform.position).normalized;
            var rotationMatrix = Matrix4x4.Rotate(Quaternion.LookRotation(currentForward, Vector3.up));

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = rotationMatrix.MultiplyPoint(vertices[i]);
        }
        private void ProjectOntoPlanetSurface(ref Vector3[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = (vertices[i] + currentForward * zValue).normalized;
                vertices[i] = shapeGenerator.GetPointOnPlanet(vertices[i]);
            }
        }
    }
}
