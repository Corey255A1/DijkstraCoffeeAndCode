// WunderVision 2023
// https://www.wundervisionengineering.com

using DijkstraAlgorithm;

Console.WriteLine("Welcome to the Dijsktra Console");

var startNode = new DijkstraNode(0, 0);
var endNode = new DijkstraNode(4, 0);

var nodeList = new List<DijkstraNode>()
{
    startNode,
    new DijkstraNode(2, -1),
    new DijkstraNode(2, -3),
    new DijkstraNode(1, 1),
    new DijkstraNode(2, 1),
    endNode
};

var edgeList = new List<Edge>() {
    new Edge(startNode, nodeList[1]),
    new Edge(startNode, nodeList[2]),
    new Edge(startNode, nodeList[3]),
    new Edge(nodeList[3], nodeList[4]),
    new Edge(nodeList[1], nodeList[4]),
    new Edge(endNode, nodeList[1]),
    new Edge(endNode, nodeList[2]),
    new Edge(endNode, nodeList[4]),
};

foreach(var node in Dijkstra.FindShortestPath(startNode, endNode))
{
    Console.WriteLine($"{node.Point.X}, {node.Point.Y}");
}