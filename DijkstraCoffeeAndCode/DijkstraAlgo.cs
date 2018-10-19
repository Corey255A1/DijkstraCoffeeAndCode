// WUNDERVISION 2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode
{
    public class DijkstraAlgo
    {
        List<Node> Nodes = new List<Node>();

        public void Add(Node n)
        {
            Nodes.Add(n);
        }
        public void Remove(Node n)
        {
            Nodes.Remove(n);
        }

        private Node currentNodeStep = null;
        private Node startNodeStep = null;
        private Node endNodeStep = null;
        private Node lastNeighbor = null;
        private int currentStepEdgeIndex = 0;
        private bool stepInProgress = false;
        public bool StepInProgress
        {
            get
            {
                return stepInProgress;
            }
        }

        public void ResetStepAlgo()
        {
            Nodes.ForEach(n => n.Reset());
            startNodeStep = null;
            currentNodeStep = null;
            endNodeStep = null;
            stepInProgress = false;
            lastNeighbor = null;
            currentStepEdgeIndex = 0;
        }

        public void BeginStepAlgo(Node startNode, Node endNode)
        {
            Nodes.ForEach(n => n.Reset());
            startNodeStep = startNode;
            currentNodeStep = startNodeStep;
            currentNodeStep.ShortestDistance = 0;
            currentNodeStep.Highlight = true;
            endNodeStep = endNode;
            stepInProgress = true;
            lastNeighbor = null;
            currentStepEdgeIndex = 0;
        }
        public void TakeStep()
        {
            if(currentNodeStep!= null)
            {
                if(currentNodeStep!=endNodeStep && !currentNodeStep.Visited)
                {
                    if(currentStepEdgeIndex<currentNodeStep.Edges.Count)
                    {                        
                        Edge edge = currentNodeStep.Edges[currentStepEdgeIndex];
                        Node neighbor = edge.GetEnd(currentNodeStep);
                        while(neighbor.Visited == true && ++currentStepEdgeIndex < currentNodeStep.Edges.Count)
                        {
                            edge = currentNodeStep.Edges[currentStepEdgeIndex];
                            neighbor = edge.GetEnd(currentNodeStep);
                        }
                        if (neighbor.Visited == false)
                        {
                            neighbor.Visualized = true;
                            if (lastNeighbor != null) lastNeighbor.Visualized = false;
                            lastNeighbor = neighbor;

                            int k = currentNodeStep.ShortestDistance + (int)edge.Distance;
                            if (k < neighbor.ShortestDistance)
                            {
                                neighbor.Shortest = currentNodeStep;
                                neighbor.ShortestDistance = k;
                            }
                        }
                        currentStepEdgeIndex++;
                    }
                    else
                    {
                        currentNodeStep.Visited = true;
                        currentNodeStep.Highlight = false;
                        Nodes.Sort((n1, n2) =>
                        {
                            if (n1.Visited && !n2.Visited)
                            {
                                return 1;
                            }
                            else if (n2.Visited && !n1.Visited)
                            {
                                return -1;
                            }
                            else
                            {
                                return n1.ShortestDistance - n2.ShortestDistance;
                            }
                        });

                        currentNodeStep = Nodes[0];
                        currentNodeStep.Highlight = true;
                        if (currentNodeStep != lastNeighbor && lastNeighbor != null)
                        {
                            lastNeighbor.Visualized = false;
                        }
                        lastNeighbor = null;
                        currentStepEdgeIndex = 0;
                    }

                }
                else
                {
                    if (currentNodeStep.Shortest == null)
                    {
                        Console.WriteLine("No Connection To End");
                    }

                    while (currentNodeStep != startNodeStep)
                    {
                        Console.WriteLine(currentNodeStep.ShortestDistance);
                        currentNodeStep.Highlight = true;
                        currentNodeStep.GetEdge(currentNodeStep.Shortest).Highlighted = true;
                        currentNodeStep = currentNodeStep.Shortest;
                    }

                    stepInProgress = false;
                }

            }
        }

        public bool FindShortestPath(Node startNode, Node endNode)
        {
            Nodes.ForEach(n => n.Reset());
            Node current = startNode;
            current.ShortestDistance = 0;

            while(current!=endNode && !current.Visited)
            {
                foreach(var edge in current.Edges)
                {
                    Node neighbor = edge.GetEnd(current);
                    if (neighbor.Visited == false)
                    {
                        int k = current.ShortestDistance + (int)edge.Distance;
                        if (k < neighbor.ShortestDistance)
                        {
                            neighbor.Shortest = current;
                            neighbor.ShortestDistance = k;
                        }
                    }
                }
                current.Visited = true;
                Nodes.Sort((n1, n2) =>
                {
                    if(n1.Visited && !n2.Visited)
                    {
                        return 1;
                    }
                    else if(n2.Visited && !n1.Visited)
                    {
                        return -1;
                    }
                    else
                    {
                        return n1.ShortestDistance - n2.ShortestDistance;
                    }
                });

                current = Nodes[0];
            }
            if(current.Shortest == null)
            {
                Console.WriteLine("No Connection To End");
                return false;
            }

            while(current != startNode)
            {
                Console.WriteLine(current.ShortestDistance);
                current.Highlight = true;
                current.GetEdge(current.Shortest).Highlighted = true;
                current = current.Shortest;
            }
            return true;


        }

    }
}
