using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    [System.Serializable]
    public class MapNode
    {
        private readonly List<MapNode> neighbours = new List<MapNode>();
        private byte numberOfNeighbours = 0;

        public Vector3 position;
        public float height;
        // internal Biome
        // internal Continent

        public MapNode(Vector3 position)
        {
            this.position = position;
            this.height = 1.5f;
        }

        public List<MapNode> GetNeighbours()
        {
            return neighbours;
        }
        public byte GetNeighboursCount()
        {
            return numberOfNeighbours;
        }
        public bool IsNeighbourWith(MapNode neighbour)
        {
            return neighbours.Contains(neighbour);
        }

        public Vector3 GetFinalPosition()
        {
            return position.normalized * height;
        }

        public bool AddNeighbour(ref MapNode neighbour)
        {
            if (numberOfNeighbours >= MapConfiguration.MAX_NEIGHBOURS || neighbour.numberOfNeighbours >= MapConfiguration.MAX_NEIGHBOURS)
            {
                Debug.LogWarning("MapNode : Adding too many neighbours!");
                return false;
            }

            if (neighbours.Contains(neighbour))
            {
                Debug.LogWarning("MapNode : Neighbour already exists!");
                return false;
            }

            this.neighbours.Add(neighbour);
            neighbour.neighbours.Add(this);

            this.numberOfNeighbours++;
            neighbour.numberOfNeighbours++;
            return true;
        }
        public bool RemoveNeighbour(ref MapNode neighbour)
        {

            if (this.numberOfNeighbours == 0 || neighbour.numberOfNeighbours == 0)
            {
                Debug.LogWarning("MapNode : Removing from empty list!");
            }
            else if (neighbours.Contains(neighbour))
            {
                neighbours.Remove(neighbour);
                neighbour.neighbours.Remove(this);

                this.numberOfNeighbours--;
                neighbour.numberOfNeighbours--;

                return true;
            }
            else
            {
                Debug.LogWarning("MapNode : Tryied to remove unexisting neighbour!");
            }

            return false;
        }
    
        public void DrawDebugLine(Vector3 origin)
        {
            Debug.DrawLine(origin + position.normalized, origin +position.normalized * height, Color.green, Time.deltaTime);
        }
        public void DrawDebugConnections(Vector3 origin)
        {
            foreach(var node in neighbours)
            {
                Debug.DrawLine(origin + position.normalized, origin + node.position.normalized, Color.cyan, Time.deltaTime);
            }
        }
    }
}

