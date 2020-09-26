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
            DATA_HEIGHT = (int)(DATA_WIDTH / 2.0f * (1 - (settings.polarAngle / 180f)));
            Generate();
        }

        public void Generate()
        {
            worldData = new GridTileData(DATA_WIDTH, DATA_HEIGHT);
            foreach (var generator in settings.continentGenerators)
            {
                var newContinent = generator.Generate();
                int yOffset = (int)(DATA_HEIGHT / 2.0f - newContinent.height / 2.0f);
                yOffset += (int)(yOffset * generator.verticalOffset);

                int xOffset = (int)(DATA_WIDTH / 2.0f - newContinent.width / 2.0f);
                xOffset += (int)(xOffset * generator.horizontaOffset);

                worldData.MergeGrid(newContinent, xOffset, yOffset);
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
            //int indexX = Mathf.RoundToInt(horizontalAngle * horizontalScale);
            //int indexY = Mathf.RoundToInt((verticalAngle - settings.polarAngle) * verticalScale);
            //var height = worldData.GetHeight(indexX, indexY);

            float x = horizontalAngle * horizontalScale;
            float y = (verticalAngle - settings.polarAngle) * verticalScale;

            int indexX = Mathf.FloorToInt(x);
            int indexY = Mathf.FloorToInt(y);

            var heightLowLeft = worldData.GetHeight(indexX, indexY);
            var heightLowRight = worldData.GetHeight(indexX + 1, indexY);
            var heightHighLeft = worldData.GetHeight(indexX, indexY + 1);
            var heightHighRight = worldData.GetHeight(indexX + 1, indexY + 1);


            byte y1 = (byte)((indexX + 1 - x) / (indexX + 1 - indexX) * heightLowLeft + (x - indexX) / (indexX + 1 - indexX) * heightLowRight);
            byte y2 = (byte)((indexX + 1 - x) / (indexX + 1 - indexX) * heightHighLeft + (x - indexX) / (indexX + 1 - indexX) * heightHighRight);
            byte height = (byte)((indexY + 1 - y) / (indexY + 1 - indexY) * y1 + (y - indexY) / (indexY + 1 - indexY) * y2);

            return pointOnUnitSphere * settings.planetRadius + pointOnUnitSphere * height / 255.0f * settings.mountainMaxHeight;
        }
    }
}
