// WUNDERVISION 2018
// https://www.wundervisionengineering.com

// WunderVision Complete Refactor in 2023

using System;
using System.Collections.Generic;
using System.Linq;
using static DijkstraAlgorithm.Dijkstra;

namespace DijkstraAlgorithm
{
    public static class Dijkstra
    {
        public static List<Node> FindShortestPath(DijkstraNode startNode, DijkstraNode endNode)
        {
            DijkstraState dijkstraState = new DijkstraState(startNode, endNode);
            while (!dijkstraState.IsFinished)
            {
                while (dijkstraState.HasNodeNeighbors)
                {
                    dijkstraState.CheckNextNeighbor();
                }
                dijkstraState.VisitNextNode();
            }
            Console.WriteLine(endNode.ShortestRouteDistance);

            return dijkstraState.GenerateShortestPathList();
        }

        public static DijkstraState TakeStep(DijkstraNode startNode, DijkstraNode endNode)
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
