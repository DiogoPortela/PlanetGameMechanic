using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PlanetData
{
    [Serializable]
    public class PixelData
    {
        public byte height;
        public BiomeType type;
    }
    public enum BiomeType : byte
    {
        ocean = 0,
        beach,
        grass,
    }

    int width;
    int height;
    PixelData[,] map;

    public PlanetData(int width, int height)
    {
        map = new PixelData[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                map[x, y] = new PixelData();
            }
        }
        this.width = width;
        this.height = height;
    }

    public byte GetHeight(int coordinateX, int coordinateY)
    {
        return map[coordinateX, coordinateY].height;
    }
    private List<PixelData> GetPixels(int coordinateX, int coordinateY, int radius)
    {
        List<PixelData> resultingPixels = new List<PixelData>();

        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                Vector2Int resultCoordinates = new Vector2Int(Modulus(coordinateX + x, width), Modulus(coordinateY + y, height));
                if (Mathf.Abs((new Vector2Int(coordinateX + x, coordinateY + y) - new Vector2Int(coordinateX, coordinateY)).magnitude) <= radius)
                {
                    resultingPixels.Add(map[resultCoordinates.x, resultCoordinates.y]);
                }
            }
        }
        return resultingPixels;
    }
    public void SetHeights(byte value, int radius, int coordinateX, int coordinateY)
    {
        var pixelsToChange = GetPixels(coordinateX, coordinateY, radius);
        foreach(var pixel in pixelsToChange)
        {
            pixel.height = value;
        }
    }
    public void MoveHeights(byte increment, int radius, int coordinateX, int coordinateY)
    {
        var pixelsToChange = GetPixels(coordinateX, coordinateY, radius);
        foreach (var pixel in pixelsToChange)
        {
            pixel.height += increment;
        }
    }

    public int Modulus(int value, int top)
    {
        int result = value % top;
        if ((result < 0 && top > 0) || (result > 0 && top < 0))
        {
            result += top;
        }
        return result;
    }
}
