// WUNDERVISION 2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace DijkstraCoffeeAndCode
{
    public enum NodeProperties { POINT, HIGHLIGHT, VIZUALIZED, VISITED, SHORTESTDISTANCE}
    public delegate void NodeUpdatedEvent(Node obj, NodeProperties prop);
    public class Node
    {
        public NodeUpdatedEvent NodeUpdated;
        public Point point;
        public List<Edge> Edges = new List<Edge>();
        public void SetPoint(double x, double y)
        {
            point.X = x;
            point.Y = y;
            NodeUpdated?.Invoke(this, NodeProperties.POINT);
        }


        public Node Shortest = null;
        private int shortestDistance = int.MaxValue;
        private bool highlighted = false;
        private bool visualized = false;
        private bool visited = false;
        public bool Visualized
        {
            get
            {
                return visualized;
            }
            set
            {
                visualized = value;
                NodeUpdated?.Invoke(this, NodeProperties.VIZUALIZED);
            }
        }
        public bool Highlight
        {
            get
            {
                return highlighted;
            }
            set
            {
                highlighted = value;
                NodeUpdated?.Invoke(this, NodeProperties.HIGHLIGHT);
            }
        }
        public bool Visited
        {
            get
            {
                return visited;
            }
            set
            {
                visited = value;
                NodeUpdated?.Invoke(this, NodeProperties.VISITED);
            }
        }
        public int ShortestDistance
        {
            get
            {
                return shortestDistance;
            }
            set
            {
                shortestDistance = value;
                NodeUpdated?.Invoke(this, NodeProperties.SHORTESTDISTANCE);
            }
        }

        public void Reset()
        {
            Visited = false;
            Visualized = false;
            Highlight = false;
            Shortest = null;
            ShortestDistance = int.MaxValue;
            Edges.ForEach(e => e.Highlighted = false);
        }

        public void RemoveEdge(Edge e)
        {
            Edges.Remove(e);
        }

        public Edge GetEdge(Node otherNode)
        {
            return Edge.GetSharedEdge(this, otherNode);
        }
       
        public void Delete()
        {
            foreach(Edge edge in Edges)
            {
                edge.GetEnd(this).RemoveEdge(edge);
                edge.Delete();
            }
        }
        public Node() { point = new Point(0, 0); }
        public Node(double x, double y)
        {
            point = new Point(x, y);
        }

        public static implicit operator Point (Node n)
        {
            return n.point;
        }




    }
}
