using System;
using UnityEngine;

[Serializable]
public class ShapeGenerator
{
    const int DATA_HEIGHT = 256;
    const int DATA_WIDTH = 512;

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
        foreach(var generator in settings.continentGenerators)
        {
            generator.Generate(ref mapData);
        }
    }
    public Vector3 GetPointOnPlanet(Vector3 pointOnUnitySphere)
    {
        pointOnUnitySphere.Normalize();
        var horizontalAngle = Vector3.SignedAngle(Vector3.forward, new Vector3(pointOnUnitySphere.x, 0, pointOnUnitySphere.z), Vector3.up);
        var verticalAngle = Vector3.Angle(Vector3.down, new Vector3(pointOnUnitySphere.x, pointOnUnitySphere.y, pointOnUnitySphere.z));

        if(horizontalAngle < 0)
        {
            horizontalAngle = 360 + horizontalAngle;
        }

        float horizontalScale = (DATA_WIDTH - 1) / 360.0f;
        float verticalScale = (DATA_HEIGHT - 1) / 180.0f;
        int pixelX = Mathf.RoundToInt(horizontalAngle * horizontalScale);
        int pixelY = Mathf.RoundToInt(verticalAngle * verticalScale);

        var height = mapData.GetHeight(pixelX, pixelY);

        return pointOnUnitySphere + pointOnUnitySphere * height / 255.0f * 0.25f;
    }
}
