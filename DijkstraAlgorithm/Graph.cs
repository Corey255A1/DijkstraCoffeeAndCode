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

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
            foreach (var edge in _edges.Where(edge => edge.Contains(node)).ToList())
            {
                RemoveEdge(edge);
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


    }
}
