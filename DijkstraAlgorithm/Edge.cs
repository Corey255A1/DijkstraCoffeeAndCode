// WUNDERVISION 2018
// https://www.wundervisionengineering.com

// WunderVision Complete Refactor in 2023

namespace DijkstraAlgorithm
{
    public class Edge
    {
        public Node Node1 { get; private set; }
        public Node Node2 { get; private set; }

        public Edge(Node node1, Node node2)
        {
            Node1 = node1;
            Node2 = node2;
        }

        public static Edge? FindSharedEdge(Node node1, Node node2)
        {
            return node1.FindSharedEdge(node2);
        }

        public double Distance
        {
            get => Node1.Distance(Node2);
        }

        public Node GetOtherNode(Node node)
        {
            if(Node1 == node) { return Node2; }
            else if(Node2 == node) { return Node1; }

            throw new Exception("Node not part of this Edge");
        }

    }
}
