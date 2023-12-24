// WunderVision 2023
// https://www.wundervisionengineering.com

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

startNode.AddEdge(nodeList[1]);
nodeList[1].AddEdge(nodeList[2]);
nodeList[2].AddEdge(endNode);
startNode.AddEdge(nodeList[3]);
nodeList[3].AddEdge(nodeList[4]);
nodeList[4].AddEdge(endNode);

nodeList[2].AddEdge(nodeList[3]);

foreach (var node in Dijkstra.FindShortestPath(startNode, endNode))
{
    Console.WriteLine($"{node.Node.Point.X}, {node.Node.Point.Y}");
}