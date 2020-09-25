using System;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [Serializable]
    public class ShapeGenerator
    {
        public const int DATA_WIDTH = 1024;
        public static int DATA_HEIGHT { get; private set; }

        GridTileData worldData;
        ShapeSettings settings;

        public ShapeGenerator(ShapeSettings settings)
        {
            this.settings = settings;
            DATA_HEIGHT = (int) (DATA_WIDTH / 2.0f *  (1 - (settings.polarAngle / 180f)));
            Generate();
        }

        public void Generate()
        {
            worldData = new GridTileData(DATA_WIDTH, DATA_HEIGHT);
            foreach (var generator in settings.continentGenerators)
            {
                worldData.MergeGrid(generator.Generate());
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

            var height = worldData.GetHeight(indexX, indexY);

            return pointOnUnitSphere * settings.planetRadius + pointOnUnitSphere * height / 255.0f * settings.mountainMaxHeight;
        }
    }
}
