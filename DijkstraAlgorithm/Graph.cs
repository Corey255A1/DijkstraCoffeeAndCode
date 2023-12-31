// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm.File;
using System.Collections.ObjectModel;

namespace DijkstraAlgorithm
{
    public class Graph
    {
        private ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes { get { return _nodes; } }

        private ObservableCollection<Edge> _edges;
        public ObservableCollection<Edge> Edges { get { return _edges; } }

        private uint _nextNodeID = 0;

        public Graph()
        {
            _nodes = new ObservableCollection<Node>();
            _edges = new ObservableCollection<Edge>();
        }

        private Graph(uint nextNodeID, IList<Node> nodes, IList<Edge> edges)
        {
            _nextNodeID = nextNodeID;
            _nodes = new ObservableCollection<Node>(nodes.Select(node=>new Node(node)));
            _edges = new ObservableCollection<Edge>();
            foreach(var edge in edges)
            {
                AddEdge(GetNodeByID(edge.Node1.ID), GetNodeByID(edge.Node2.ID));
            }
        }

        public static Graph LoadGraph(string filePath)
        {
            GraphFile graphFile = GraphFile.LoadGraph(filePath);
            Graph graph = new Graph();
            foreach (var node in graphFile.Nodes)
            {
                graph.AddNode(node.ID, node.X, node.Y);
            }

            foreach (var edge in graphFile.Edges)
            {
                graph.AddEdge(edge.Node1ID, edge.Node2ID);
            }

            return graph;
        }

        public void ImportGraph(string filePath)
        {
            GraphFile graphFile = GraphFile.LoadGraph(filePath);
            // When importing, the IDs cannot be the IDs already in the graph.
            // use the initial ID as the offset for the Edges
            uint nodeIDOffset = _nextNodeID;
            foreach (var node in graphFile.Nodes)
            {
                AddNode(nodeIDOffset + node.ID, node.X, node.Y);
            }

            foreach (var edge in graphFile.Edges)
            {
                AddEdge(nodeIDOffset + edge.Node1ID, nodeIDOffset + edge.Node2ID);
            }
        }

        public static void SaveGraph(Graph graph, string filePath)
        {
            GraphFile.SaveGraph(new GraphFile(graph), filePath);
        }

        private uint GetNodeID()
        {
            _nextNodeID += 1;
            return _nextNodeID;
        }

        private void AddNode(uint id, double x, double y)
        {
            _nextNodeID = Math.Max(id, _nextNodeID);
            Node node = new(id, x, y);
            _nodes.Add(node);
        }

        public void AddNode(double x, double y)
        {
            AddNode(GetNodeID(), x, y);
        }

        public Node GetNodeByID(uint id)
        {
            return Nodes.First(n => n.ID == id);
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
            foreach (var node in _nodes.ToList())
            {
                RemoveNode(node);
            }
        }

        public void AddEdge(uint node1ID, uint node2ID)
        {
            AddEdge(GetNodeByID(node1ID), GetNodeByID(node2ID));
        }

        public void AddEdge(Node node1, Node node2)
        {
            Edge? edge = node1.MakeEdge(node2);
            if (edge == null) { return; }

            _edges.Add(edge);
        }

        public void RemoveEdge(Edge edge)
        {
            edge.Clear();
            _edges.Remove(edge);
        }

        public void RemoveEdge(Node node1, Node node2)
        {
            Edge? edge = _edges.FirstOrDefault(edge => edge.Contains(node1, node2));
            if (edge == null) { return; }

            RemoveEdge(edge);
        }

        public void RemoveAllEdges()
        {
            foreach (var edge in _edges.ToList())
            {
                RemoveEdge(edge);
            }
        }

        public Graph Copy()
        {
            return new Graph(_nextNodeID, _nodes, _edges);
        }

    }
}
