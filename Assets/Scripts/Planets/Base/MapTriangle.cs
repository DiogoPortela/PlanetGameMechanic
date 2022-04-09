using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pt.dportela.PlanetGame.PlanetGeneration
{
    public class MapTriangle
    {
        private MapNode[] vertices = new MapNode[3];
        private MapTriangle parent = null;
        private List<MapTriangle> children = new List<MapTriangle>();

        public MapTriangle(MapNode vert1, MapNode vert2, MapNode vert3, MapTriangle parent = null)
        {
            this.vertices[0] = vert1;
            this.vertices[1] = vert2;
            this.vertices[2] = vert3;

            if(parent != null)
            {
                parent.AddChildren(this);
            }
        }

        public bool AddChildren(MapTriangle child)
        {
            if (children.Contains(child))
            {
                Debug.LogWarning($"MapTriangle: child already exists child={child}");
                return false;
            }

            this.children.Add(child);
            child.parent = this;

            return true;
        }
        public List<MapTriangle> GetChildren()
        {
            List<MapTriangle> result = new List<MapTriangle>(); ;
            if(children.Count > 0)
            {
                foreach (var child in children)
                {
                    result.AddRange(child.GetChildren());
                }
            }
            else
            {
                result.Add(this);
            }

            return result;
        }
        public MapNode[] GetVertices()
        {
            return vertices;
        }

        public MapTriangle GetTriangleForPosition(Vector3 position)
        {
            var result = this;

            foreach(var tri in children)
            {
                var triNodes = tri.vertices;

                Plane x = new Plane(triNodes[0].position, triNodes[1].position, triNodes[2].position);
                //x.
                //if ()
            }

            return result;
        }
        public bool IsLineInsideTriangle(Vector3 direction)
        {
            return true;
        }
        public void DebugDrawTriangle(Vector3 origin)
        {
            Color color = Color.red;

            Debug.DrawLine(origin + vertices[0].position.normalized * vertices[0].height, origin + vertices[1].position.normalized * vertices[1].height, color, Time.deltaTime);
            Debug.DrawLine(origin + vertices[1].position.normalized * vertices[1].height, origin + vertices[2].position.normalized * vertices[2].height, color, Time.deltaTime);
            Debug.DrawLine(origin + vertices[2].position.normalized * vertices[2].height, origin + vertices[0].position.normalized * vertices[0].height, color, Time.deltaTime);
        }
    }
}

