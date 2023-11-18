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
        public class DijkstraState
        {
            public DijkstraNode StartNode { get; set; }
            public DijkstraNode EndNode { get; set; }
            public DijkstraNode CurrentNode { get; set; }
            
            private Queue<DijkstraNode> _currentNodeNeighbors;
            public Queue<DijkstraNode> CurrentNodeNeighbors => _currentNodeNeighbors;

            public bool HasNodeNeighbors => _currentNodeNeighbors.Count > 0;

            private HashSet<DijkstraNode> _nodesToVisit = new ();

            public bool IsFinished => CurrentNode == EndNode || CurrentNode.Visited;

            public DijkstraState(DijkstraNode startNode, DijkstraNode endNode)
            {
                StartNode = startNode;
                EndNode = endNode;
                CurrentNode = startNode;
                CurrentNode.ShortestRouteDistance = 0;
                _currentNodeNeighbors = new Queue<DijkstraNode>(CurrentNode.UnvisitedNodes);
            }

            public void AddNodeToVisit(DijkstraNode node)
            {
                _nodesToVisit.Add(node);
            }

            public DijkstraNode GetNextNodeNeighbor()
            {
                return _currentNodeNeighbors.Dequeue();
            }

            public void CheckNextNeighbor()
            {
                DijkstraNode neighborNode = GetNextNodeNeighbor();
                neighborNode.UpdateShortestRoute(CurrentNode);
                AddNodeToVisit(neighborNode);
            }

            public void VisitNextNode()
            {
                CurrentNode.Visited = true;
                CurrentNode = _nodesToVisit.Min((node) => node);
                _nodesToVisit.Remove(CurrentNode);
                _currentNodeNeighbors = new Queue<DijkstraNode>(CurrentNode.UnvisitedNodes);
            }

            public List<Node> GenerateShortestPathList()
            {
                List<Node> shortestPath = new List<Node>();
                shortestPath.Add(EndNode);
                DijkstraNode currentNode = EndNode;

                while (currentNode != null && currentNode != StartNode)
                {
                    currentNode = currentNode.ShortestRouteNode;
                    shortestPath.Add(currentNode);
                }

                shortestPath.Reverse();
                return shortestPath;
            }
        }

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
