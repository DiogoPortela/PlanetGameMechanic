using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [CreateAssetMenu()]
    public class MapColorSettings : ScriptableObject
    {
        public Material material;
        public Color planetColor;
    }
}
