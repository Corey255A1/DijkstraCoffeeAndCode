using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DijkstraAlgorithm
{
    public class DijkstraState
    {
        public DijkstraNode StartNode { get; set; }
        public DijkstraNode EndNode { get; set; }
        public DijkstraNode? CurrentNode { get; set; }
        public DijkstraNode? LastCheckedNeighbor { get; set; }

        private Queue<DijkstraNode> _currentNodeNeighbors;
        public Queue<DijkstraNode> CurrentNodeNeighbors => _currentNodeNeighbors;

        public bool HasNodeNeighbors => _currentNodeNeighbors.Count > 0;

        private HashSet<DijkstraNode> _nodesToVisit = new();
        private Dictionary<Node, DijkstraNode> _dijkstraNodes = new();

        public IEnumerable<DijkstraNode> DijkstraNodes => _dijkstraNodes.Values;

        public bool IsFinished => CurrentNode == null || CurrentNode == EndNode || CurrentNode.Visited;

        public DijkstraState(Node startNode, Node endNode)
        {
            DijkstraNode dijkstraStartNode = new(startNode);
            DijkstraNode dijkstraEndNode = new(endNode);
            Construct(dijkstraStartNode, dijkstraEndNode);
        }
        public DijkstraState(DijkstraNode startNode, DijkstraNode endNode)
        {
            Construct(startNode, endNode);
        }



        private void Construct(DijkstraNode startNode, DijkstraNode endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            _dijkstraNodes[startNode.Node] = startNode;
            _dijkstraNodes[endNode.Node] = endNode;

            CurrentNode = startNode;
            CurrentNode.ShortestRouteDistance = 0;
            LastCheckedNeighbor = null;
            _currentNodeNeighbors = new Queue<DijkstraNode>(GetOrCreateDijkstraNode(CurrentNode.Node.Neighbors));
        }

        private DijkstraNode GetOrCreateDijkstraNode(Node node)
        {
            if (!_dijkstraNodes.ContainsKey(node))
            {
                _dijkstraNodes[node] = new DijkstraNode(node);
            }
            return _dijkstraNodes[node];
        }

        private IEnumerable<DijkstraNode> GetOrCreateDijkstraNode(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                yield return GetOrCreateDijkstraNode(node);
            }
        }

        private IEnumerable<DijkstraNode> GetUnvisitedNeighborNodes(DijkstraNode node)
        {
            return GetOrCreateDijkstraNode(node.Node.Neighbors.Where(
               (baseNode) =>
               {
                   if (!_dijkstraNodes.ContainsKey(baseNode)) { return true; }
                   return !_dijkstraNodes[baseNode].Visited;
               }));
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
            if (CurrentNode == null) { return; }

            LastCheckedNeighbor = GetNextNodeNeighbor();
            if (LastCheckedNeighbor == null) { return; }


            LastCheckedNeighbor.UpdateShortestRoute(CurrentNode);
            AddNodeToVisit(LastCheckedNeighbor);
        }

        public void VisitNextNode()
        {
            if (CurrentNode == null) { return; }

            CurrentNode.Visited = true;
            CurrentNode = _nodesToVisit.Min((node) => node);
            if (CurrentNode == null) { return; }

            _nodesToVisit.Remove(CurrentNode);
            _currentNodeNeighbors = new Queue<DijkstraNode>(GetUnvisitedNeighborNodes(CurrentNode));
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
