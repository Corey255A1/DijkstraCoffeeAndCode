using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class DijkstraGraph
    {
        private ObservableCollection<DijkstraNode> _nodes;
        public ObservableCollection<DijkstraNode> Nodes { get { return _nodes; } }

        private ObservableCollection<Edge> _edges;
        public ObservableCollection<Edge> Edges { get { return _edges; } }

        public DijkstraGraph()
        {
            _nodes = new ObservableCollection<DijkstraNode>();
            _edges = new ObservableCollection<Edge>();
        }

        public void AddNode(double x, double y)
        {
            DijkstraNode node = new(x, y);
            _nodes.Add(node);
        }

        public void RemoveNode(DijkstraNode node)
        {
            _nodes.Remove(node);
            foreach (var edge in _edges.Where(edge => edge.Contains(node)).ToList())
            {
                RemoveEdge(edge);
            }
        }

        public void AddEdge(DijkstraNode node1, DijkstraNode node2)
        {
            Edge? edge = node1.AddEdge(node2);
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
