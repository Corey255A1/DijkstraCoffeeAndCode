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

        private ObservableCollection<Edge> _edges = new ObservableCollection<Edge>();
        public ObservableCollection<Edge> Edges => _edges;
        public IEnumerable<Node> Neighbors => _edges.ToList().ConvertAll((edge) => edge.GetOtherNode(this));

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

        public double Distance(Node node)
        {
            return _point.Distance(node._point);
        }

        public Edge? MakeEdge(Node node)
        {
            Edge? edge = FindSharedEdge(node);
            if (edge != null) { return null; }

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
            return _edges.ToList().Find(e => e.GetOtherNode(this) == otherNode);
        }

        public void RemoveAllEdges()
        {
            foreach (Edge edge in _edges)
            {
                edge.GetOtherNode(this).RemoveEdge(edge);
            }
        }
    }
}
