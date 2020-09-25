using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [CreateAssetMenu()]
    public class ShapeSettings : ScriptableObject
    {
        public float planetRadius = 1;
        public bool isMoon = false;
        [Range(0, 1)]
        public float mountainMaxHeight = 0.25f;
        public float polarAngle = 20.0f;
        public List<ContinentGenerator> continentGenerators;
    }
}
