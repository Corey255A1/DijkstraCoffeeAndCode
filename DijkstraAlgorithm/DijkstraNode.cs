using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class DijkstraNode : Node, IComparable<DijkstraNode>
    {
        private bool _visited;

        public bool Visited
        {
            get { return _visited; }
            set { _visited = value; }
        }

        private double _shortesRouteDistance = double.MaxValue;

        public double ShortestRouteDistance
        {
            get { return _shortesRouteDistance; }
            set { _shortesRouteDistance = value; }
        }

        private DijkstraNode? _shortestRouteNode;

        public DijkstraNode? ShortestRouteNode
        {
            get { return _shortestRouteNode; }
            set { _shortestRouteNode = value; }
        }

        public IEnumerable<DijkstraNode> UnvisitedNodes => Nodes.Cast<DijkstraNode>().Where(node => !node.Visited);

        public static bool operator <(DijkstraNode left, DijkstraNode right)
        {
            return left.ShortestRouteDistance < right.ShortestRouteDistance;
        }

        public static bool operator >(DijkstraNode left, DijkstraNode right)
        {
            return left.ShortestRouteDistance > right.ShortestRouteDistance;
        }

        public DijkstraNode(double x, double y) : base(x, y) { }

        public int CompareTo(DijkstraNode? other)
        {
            if (other == null) { throw new ArgumentNullException("other"); }
            if (other == this) { return 0; }
            return (int)(ShortestRouteDistance - other.ShortestRouteDistance);
        }

        public void UpdateShortestRoute(DijkstraNode node)
        {
            double nextRouteDistance = node.ShortestRouteDistance + Distance(node);
            if (nextRouteDistance >= ShortestRouteDistance)
            {
                return;
            }
            ShortestRouteNode = node;
            ShortestRouteDistance = nextRouteDistance;
        }

        public void Reset()
        {
            ShortestRouteNode = null;
            ShortestRouteDistance = double.MaxValue;
            Visited = false;
        }
    }
}
