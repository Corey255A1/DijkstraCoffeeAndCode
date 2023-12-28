using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Graph
    {
        private ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes { get { return _nodes; } }

        private ObservableCollection<Edge> _edges;
        public ObservableCollection<Edge> Edges { get { return _edges; } }

        public Graph()
        {
            _nodes = new ObservableCollection<Node>();
            _edges = new ObservableCollection<Edge>();
        }

        public void AddNode(double x, double y)
        {
            Node node = new(x, y);
            _nodes.Add(node);
        }

        // Remove the edges associated from the node
        // before triggering the Node removed callbacks.
        // Otherwise, the nodes will still have those connections
        public void RemoveNode(Node node)
        {
            foreach (var edge in _edges.Where(edge => edge.Contains(node)).ToList())
            {
                RemoveEdge(edge);
            }
            _nodes.Remove(node);
        }

        public void RemoveAllNodes()
        {
            foreach(var node in _nodes.ToList())
            {
                RemoveNode(node);
            }
        }

        public void AddEdge(Node node1, Node node2)
        {
            Edge? edge = node1.MakeEdge(node2);
            if(edge == null) { return; }

            _edges.Add(edge);
        }

        public void RemoveEdge(Edge edge)
        {
            edge.Clear();
            _edges.Remove(edge);
        }

        public void RemoveEdge(Node node1, Node node2)
        {
            Edge? edge = node1.FindSharedEdge(node2);
            if (edge == null) { return; }

            RemoveEdge(edge);
        }


    }
}
