using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [CreateAssetMenu()]
    public class MapShapeSettings : ScriptableObject
    {
        public int seed;
        [Min(1)] public float planetBaseRadius = 1.0f;
        [Range(0, 1)] public float mountainMaxHeight = 1.0f;

        public bool isMoon = false;
    }
}
