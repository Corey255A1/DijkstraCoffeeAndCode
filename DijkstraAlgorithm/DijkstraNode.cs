// WunderVision 2023
// https://www.wundervisionengineering.com/
namespace DijkstraAlgorithm
{
    public class DijkstraNode : IComparable<DijkstraNode>
    {
        private bool _visited;
        public bool IsVisited
        {
            get { return _visited; }
            set { _visited = value; }
        }

        private double _routeSegmentDistance = double.MaxValue;
        public double RouteSegmentDistance
        {
            get { return _routeSegmentDistance; }
            set { _routeSegmentDistance = value; }
        }

        private DijkstraNode? _shortestRouteNode;
        public DijkstraNode? ShortestRouteNode
        {
            get { return _shortestRouteNode; }
            set { _shortestRouteNode = value; }
        }

        private Node _node;
        public Node Node { get { return _node; } }

        public DijkstraNode(double x, double y)
        {
            _node = new Node(x, y);
        }

        public DijkstraNode(Node node)
        {
            _node = node;
        }

        public static bool operator <(DijkstraNode left, DijkstraNode right)
        {
            return left.RouteSegmentDistance < right.RouteSegmentDistance;
        }

        public static bool operator >(DijkstraNode left, DijkstraNode right)
        {
            return left.RouteSegmentDistance > right.RouteSegmentDistance;
        }



        public int CompareTo(DijkstraNode? other)
        {
            if (other == null) { throw new ArgumentNullException("other"); }
            if (other == this) { return 0; }
            return (int)(RouteSegmentDistance - other.RouteSegmentDistance);
        }

        public void UpdateShortestRoute(DijkstraNode node)
        {
            double nextRouteDistance = node.RouteSegmentDistance + _node.Distance(node.Node);
            if (nextRouteDistance >= RouteSegmentDistance)
            {
                return;
            }
            ShortestRouteNode = node;
            RouteSegmentDistance = nextRouteDistance;
        }

        public void Reset()
        {
            ShortestRouteNode = null;
            RouteSegmentDistance = double.MaxValue;
            IsVisited = false;
        }
    }
}
