using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    internal static class MapConfiguration
    {
        public const int MAX_NEIGHBOURS = 6;            //There are 6 vertices connected to each vertice.
        public const int MAX_DIVISIONS = 5;             //Number of times to divide the iicosahedron
        public const int ICOSAHEDRON_EDGES = 30;
        public const int ICOSAHEDRON_VERTICES = 12;
        public const int ICOSAHEDRON_FACES = 20;
    }
}

