using pt.dportela.PlanetGame.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [System.Serializable]
    public class ContinentGenerator
    {
        [Range(0.1f, 1)]
        public float widthScale = 1;
        [Range(0.1f, 1)]
        public float heightScale = 1;

        public float mainIslandsCount = 1;
        public float smallIslandsCount = 0;


        private List<PlanetData.PixelData> continentPixels;
        private PlanetData.PixelData[,] continentPixelData;

        internal PlanetData.PixelData[,] Generate()
        {
            int width = (int)(widthScale * ShapeGenerator.DATA_WIDTH);
            int height = (int)(heightScale * ShapeGenerator.DATA_HEIGHT);
            continentPixelData = new PlanetData.PixelData[width, height];
            Noise noise = new Noise((int)DateTime.Now.Ticks);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    PlanetData.PixelData data = new PlanetData.PixelData();
                    //data.height = noise.Evaluate(new Vector3(x, y, 0));
                    continentPixelData[x, y] = data;
                }

            }

            return continentPixelData;

            //continentPixels = new List<PlanetData.PixelData>();
            //continentPixels.AddRange(planetData.MoveHeights(10, 20, 1024, 512 / 2));


            //List<Agent> allAgents = new List<Agent>();

            //for(int i = 0; i < 5; i++)
            //{
            //    Agent agent = new Agent(position, 40, 80, 20, 50, verticalStretch, horizontalStretch);
            //    allAgents.Add(agent);
            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    foreach(var agent in allAgents)
            //    {
            //        continentPixels.AddRange(agent.Move(ref planetData));
            //    }
            //}


        }
    }
}