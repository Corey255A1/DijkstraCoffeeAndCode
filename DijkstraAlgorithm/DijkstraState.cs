using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class DijkstraState
    {
        public DijkstraNode StartNode { get; set; }
        public DijkstraNode EndNode { get; set; }
        public DijkstraNode CurrentNode { get; set; }
        public DijkstraNode? LastCheckedNeighbor { get; set; }

        private Queue<DijkstraNode> _currentNodeNeighbors;
        public Queue<DijkstraNode> CurrentNodeNeighbors => _currentNodeNeighbors;

        public bool HasNodeNeighbors => _currentNodeNeighbors.Count > 0;

        private HashSet<DijkstraNode> _nodesToVisit = new();

        public bool IsFinished => CurrentNode == EndNode || CurrentNode.Visited;

        public DijkstraState(DijkstraNode startNode, DijkstraNode endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            CurrentNode = startNode;
            CurrentNode.ShortestRouteDistance = 0;
            LastCheckedNeighbor = null;
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
            LastCheckedNeighbor = GetNextNodeNeighbor();
            if(LastCheckedNeighbor == null) { return; }

            LastCheckedNeighbor.UpdateShortestRoute(CurrentNode);
            AddNodeToVisit(LastCheckedNeighbor);
        }

        public void VisitNextNode()
        {
            CurrentNode.Visited = true;
            CurrentNode = _nodesToVisit.Min((node) => node);
            _nodesToVisit.Remove(CurrentNode);
            _currentNodeNeighbors = new Queue<DijkstraNode>(CurrentNode.UnvisitedNodes);
        }

        public List<DijkstraNode> GenerateShortestPathList()
        {
            List<DijkstraNode> shortestPath = new List<DijkstraNode>() { EndNode };
            DijkstraNode currentNode = EndNode;

            while (currentNode != null && currentNode != StartNode)
            {
                if (currentNode.ShortestRouteNode == null) { throw new Exception("Invalid Graph"); }
                currentNode = currentNode.ShortestRouteNode;                
                shortestPath.Add(currentNode);
            }

            shortestPath.Reverse();
            return shortestPath;
        }
    }
}
