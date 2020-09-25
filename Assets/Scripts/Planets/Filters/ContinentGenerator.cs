using pt.dportela.PlanetGame.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [System.Serializable]
    public class ContinentGenerator
    {
        [Range(0.01f, 1)]
        public float widthScale = 1;
        [Range(0.01f, 1)]
        public float heightScale = 1;

        [Range(-1, 1)]
        public float verticalOffset = 0;
        [Range(-1, 1)]
        public float horizontaOffset = 0;

        public float mainIslandsCount = 1;
        public float smallIslandsCount = 0;

        private GridTileData continentTileData;
        public Texture2D debugTexture;

        internal GridTileData Generate()
        {
            int width = (int)(widthScale * ShapeGenerator.DATA_WIDTH);
            int height = (int)(heightScale * ShapeGenerator.DATA_HEIGHT);
            continentTileData = new GridTileData(width, height);
            Noise noise = new Noise((int)DateTime.Now.Ticks);

            debugTexture = new Texture2D(width, height);

            float noiseScale = 50.0f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridTileData.TileData data = new GridTileData.TileData();
                    float distanceToCenter = 2 * Mathf.Max(Mathf.Abs(x / (float)width - 0.5f), Math.Abs(y / (float)height - 0.5f));

                    float a = 3;
                    float b = 2.2f;
                    distanceToCenter = Mathf.Pow(distanceToCenter, a) / (Mathf.Pow(distanceToCenter, a) + Mathf.Pow(b - b * distanceToCenter, a));

                    float elevation = (noise.Evaluate(new Vector3(x / noiseScale, y / noiseScale, 0)) 
                        + 0.25f * noise.Evaluate(new Vector3(x / noiseScale / 0.5f, y / noiseScale / 0.5f, 0)) 
                        + 0.125f * noise.Evaluate(new Vector3(x / noiseScale / 0.25f, y / noiseScale / 0.25f, 0)) 
                        + 1) * 0.5f;  // 0 to 1 elevation.

                    //debugTexture.SetPixel(x, y, new Color(elevation, elevation, elevation));

                    //elevation = (1.0f + elevation - distanceToCenter * 2.2f) / 2.0f;
                    elevation =  Mathf.Clamp(elevation - distanceToCenter, 0, 1);

                    if (elevation <= 0)
                    {
                        data.height = 255;
                        data.type = GridTileData.BiomeType.ocean;
                    }
                    else if (elevation > 1)
                    {
                        data.height = 255;
                    }
                    else
                        data.height = (byte)(elevation * 255);

                    continentTileData.map[x, y] = data;
                    debugTexture.SetPixel(x, y, new Color(data.height/255.0f, data.height/255.0f, data.height/255.0f));
                    //debugTexture.SetPixel(x, y, new Color(distanceToCenter, distanceToCenter, distanceToCenter));
                }
            }

            Queue<TileToAnalyse> tilesToAnalyse = new Queue<TileToAnalyse>();
            GridTileData analisedTileData = new GridTileData(width, height);

            tilesToAnalyse.Enqueue(new TileToAnalyse(continentTileData.map[width / 2, height / 2], width / 2, height / 2));
            while (tilesToAnalyse.Count > 0)
            {
                var currentTile = tilesToAnalyse.Dequeue();


            }

            debugTexture.Apply();
            return continentTileData;
        }



        class TileToAnalyse
        {
            public GridTileData.TileData tile;
            public int x;
            public int y;
            public TileToAnalyse(GridTileData.TileData tile, int x, int y)
            {
                this.tile = tile;
                this.x = x;
                this.y = y;
            }
        }
    }
}