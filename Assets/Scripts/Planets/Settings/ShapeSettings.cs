using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1;
    public bool hasOcean = true;
    public ContinentGenerator[] continentGenerators; 
}
