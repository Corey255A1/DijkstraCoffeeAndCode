// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;

Console.WriteLine("Welcome to the Dijsktra Console");

var startNode = new Node(0, 0);
var endNode = new Node(4, 0);

var nodeList = new List<Node>()
{
    startNode,
    new Node(2, -1),
    new Node(2, -3),
    new Node(1, 1),
    new Node(2, 1),
    endNode
};

startNode.MakeEdge(nodeList[1]);
nodeList[1].MakeEdge(nodeList[2]);
nodeList[2].MakeEdge(endNode);
startNode.MakeEdge(nodeList[3]);
nodeList[3].MakeEdge(nodeList[4]);
nodeList[4].MakeEdge(endNode);

nodeList[2].MakeEdge(nodeList[3]);
var dijkstraState = Dijkstra.FindShortestPath(startNode, endNode);
foreach (var node in dijkstraState.GenerateShortestPathList())
{
    Console.WriteLine($"{node.Node.Point.X}, {node.Node.Point.Y}");
}

Console.WriteLine($"Shortest Distance {dijkstraState.EndNode.RouteSegmentDistance}");