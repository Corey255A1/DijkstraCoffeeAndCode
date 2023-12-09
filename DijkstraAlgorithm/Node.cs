﻿// WUNDERVISION 2018
// https://www.wundervisionengineering.com

// WunderVision Complete Refactor in 2023
namespace DijkstraAlgorithm
{
    public class Node
    {
        private Vector2D _point;
        public Vector2D Point => _point;

        private List<Edge> _edges = new List<Edge>();
        public IEnumerable<Edge> Edges => _edges;
        public IEnumerable<Node> Nodes => _edges.ConvertAll((edge) => edge.GetOtherNode(this));

        public Node()
        {
            _point = new Vector2D(0, 0);
        }

        public Node(double x, double y)
        {
            _point = new Vector2D(x, y);
        }

        public double Distance(Node node)
        {
            return _point.Distance(node._point);
        }

        public Edge AddEdge(Node node)
        {
            Edge? edge = FindSharedEdge(node);
            if (edge != null) { return edge; }

            edge = new Edge(this, node);
            AddEdge(edge);
            node.AddEdge(edge);
            return edge;
        }

        private void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        public void RemoveEdge(Node node)
        {
            Edge? edge = FindSharedEdge(node);
            if (edge == null) { return; }

            RemoveEdge(edge);
            node.RemoveEdge(edge);
        }

        private void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
        }

        public Edge? FindSharedEdge(Node otherNode)
        {
            return _edges.Find(e => e.GetOtherNode(this) == otherNode);
        }

        public void Delete()
        {
            foreach (Edge edge in _edges)
            {
                edge.GetOtherNode(this).RemoveEdge(edge);
            }
        }
    }
}
