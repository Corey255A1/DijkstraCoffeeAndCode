// WunderVision 2023
// https://www.wundervisionengineering.com/
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

        public double Distance
        {
            get => Node1.Distance(Node2);
        }

        public bool Contains(Node node)
        {
            if (Node1 == node) { return true; }
            if (Node2 == node) { return true; }

            return false;
        }

        public bool Contains(Node node1, Node node2)
        {
            return Contains(node1) && Contains(node2);
        }

        public Node GetOtherNode(Node node)
        {
            if (Node1 == node) { return Node2; }
            else if (Node2 == node) { return Node1; }

            throw new Exception("Node not part of this Edge");
        }

        public void Clear()
        {
            Node1.RemoveEdge(Node2);
        }

    }
}
