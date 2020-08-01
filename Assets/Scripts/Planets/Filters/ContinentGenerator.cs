using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContinentGenerator
{
    public float size;
    public float mainIslandsCount;
    public float smallIslandsCount;

    public void Generate(ref PlanetData planetData)
    {
        planetData.MoveHeights(50, 20, 512, 256 / 2);
    }
}
