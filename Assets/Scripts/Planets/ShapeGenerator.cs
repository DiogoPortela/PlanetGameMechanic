using System;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [Serializable]
    public class ShapeGenerator
    {
        public const int DATA_WIDTH = 1024;
        public const int DATA_HEIGHT = 512;

        PlanetData mapData;
        ShapeSettings settings;

        public ShapeGenerator(ShapeSettings settings)
        {
            this.settings = settings;
            GenerateMap();
        }

        public void GenerateMap()
        {
            mapData = new PlanetData(DATA_WIDTH, DATA_HEIGHT);
            foreach (var generator in settings.continentGenerators)
            {
                mapData.AddContinent(generator.Generate());
            }
        }
        public Vector3 GetPointOnPlanet(Vector3 pointOnUnitSphere)
        {
            pointOnUnitSphere.Normalize();
            var horizontalAngle = Vector3.SignedAngle(Vector3.forward, new Vector3(pointOnUnitSphere.x, 0, pointOnUnitSphere.z), Vector3.up);
            var verticalAngle = Vector3.Angle(Vector3.down, new Vector3(pointOnUnitSphere.x, pointOnUnitSphere.y, pointOnUnitSphere.z));

            if (horizontalAngle < 0)
            {
                horizontalAngle = 360 + horizontalAngle;
            }

            float horizontalScale = (DATA_WIDTH - 1) / 360.0f;
            float verticalScale = (DATA_HEIGHT - 1) / (180.0f - settings.polarAngle * 2);
            int indexX = Mathf.RoundToInt(horizontalAngle * horizontalScale);
            int indexY = Mathf.RoundToInt((verticalAngle - settings.polarAngle) * verticalScale);

            var height = mapData.GetHeight(indexX, indexY);

            return pointOnUnitSphere * settings.planetRadius + pointOnUnitSphere * height / 255.0f * settings.mountainMaxHeight;
        }
    }
}
