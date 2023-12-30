// WunderVision 2023
// https://www.wundervisionengineering.com/

namespace DijkstraAlgorithm
{
    public static class Dijkstra
    {
        public static DijkstraState FindShortestPath(Node startNode, Node endNode)
        {
            DijkstraState dijkstraState = new(startNode, endNode);
            while (!dijkstraState.IsFinished)
            {
                while (dijkstraState.HasNodeNeighbors)
                {
                    dijkstraState.CheckNextNeighbor();
                }
                dijkstraState.VisitNextNode();
            }
            return dijkstraState;
        }

        public static DijkstraState TakeStep(Node startNode, Node endNode)
        {
            return TakeStep(new DijkstraState(startNode, endNode));
        }

        public static DijkstraState TakeStep(DijkstraState dijkstraState)
        {
            if (!dijkstraState.IsFinished)
            {
                if (dijkstraState.HasNodeNeighbors)
                {
                    dijkstraState.CheckNextNeighbor();
                    return dijkstraState;
                }
                dijkstraState.VisitNextNode();
                return dijkstraState;
            }
            return dijkstraState;
        }

    }
}
