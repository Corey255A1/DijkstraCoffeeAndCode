// WUNDERVISION 2018
// https://www.wundervisionengineering.com

// WunderVision Complete Refactor in 2023

namespace DijkstraAlgorithm
{
    public class Edge
    {
        private Node _node1;
        private Node _node2;

        public Edge(Node node1, Node node2)
        {
            _node1 = node1;
            _node1.AddEdge(this);

            _node2 = node2;
            _node2.AddEdge(this);
        }

        public static Edge FindSharedEdge(Node node1, Node node2)
        {
            return node1.FindSharedEdge(node2);
        }

        public double Distance
        {
            get => _node1.Distance(_node2);
        }

        public Node GetOtherNode(Node node)
        {
            if(_node1 == node) { return _node2; }
            else if(_node2 == node) { return _node1; }

            throw new Exception("Node not part of this Edge");
        }

    }
}
