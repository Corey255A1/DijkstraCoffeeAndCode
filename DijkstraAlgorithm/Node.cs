// WunderVision 2023
// https://www.wundervisionengineering.com/
using System.Collections.ObjectModel;

namespace DijkstraAlgorithm
{
    public class Node
    {
        private uint _id;
        public uint ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private Vector2D _point;
        public Vector2D Point => _point;

        private ObservableCollection<Node> _neighbors = new ObservableCollection<Node>();
        public ObservableCollection<Node> Neighbors => _neighbors;

        public Node()
        {
            _id = 0;
            _point = new Vector2D(0, 0);
        }

        public Node(double x, double y)
        {
            _id = 0;
            _point = new Vector2D(x, y);
        }

        public Node(uint id, double x, double y)
        {
            _id = id;
            _point = new Vector2D(x, y);
        }

        public Node(Node node)
        {
            _id = node._id;
            _point = new Vector2D(node.Point.X, node.Point.Y);
        }

        public double Distance(Node node)
        {
            return _point.Distance(node._point);
        }

        private void AddNeighbor(Node node)
        {
            _neighbors.Add(node);
        }

        public Edge? MakeEdge(Node node)
        {
            if (_neighbors.Contains(node)) { return null; }

            var edge = new Edge(this, node);
            AddNeighbor(node);
            node.AddNeighbor(this);
            return edge;
        }

        public void RemoveEdge(Node node)
        {
            if (!_neighbors.Contains(node)) { return; }

            RemoveNeighbor(node);
            node.RemoveNeighbor(this);
        }

        private void RemoveNeighbor(Node node)
        {
            _neighbors.Remove(node);
        }

        public void RemoveAllEdges()
        {
            foreach (var node in _neighbors)
            {
                node.RemoveNeighbor(this);
            }
            _neighbors.Clear();
        }
    }
}
