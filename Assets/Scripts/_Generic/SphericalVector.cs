using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace pt.dportela.PlanetGame.Utils
{
    public struct SphericalVector
    {
        public readonly float AzimuthalAngle;
        public readonly float Distance;
        public readonly float PolarAngle;

        public SphericalVector(Vector3 coordinates)
        {
            this.Distance = Mathf.Sqrt(Mathf.Pow(coordinates.x, 2) + Mathf.Pow(coordinates.y, 2) + Mathf.Pow(coordinates.z, 2));
            this.AzimuthalAngle = Mathf.Atan(coordinates.z / coordinates.x);

            float r = Mathf.Sqrt(Mathf.Pow(coordinates.x, 2) + Mathf.Pow(coordinates.z, 2));
            this.PolarAngle = Mathf.Acos(coordinates.y / Mathf.Sqrt(Mathf.Pow(r, 2) + Mathf.Pow(coordinates.y, 2)));
        }

        public Vector3 ToVector()
        {
            float x = this.Distance * Mathf.Sin(this.AzimuthalAngle) * Mathf.Cos(this.PolarAngle);
            float y = this.Distance * Mathf.Sin(this.AzimuthalAngle) * Mathf.Sin(this.PolarAngle);
            float z = this.Distance * Mathf.Cos(this.AzimuthalAngle);

            return new Vector3(x, y, z);
        }
    }
}

