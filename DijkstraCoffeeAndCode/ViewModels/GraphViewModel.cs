using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        public const int MAX_SELECTED_NODES = 2;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        private Graph _dijkstraGraph = new Graph();
        private DijkstraNodeViewModel? _startNode = null;
        public DijkstraNodeViewModel? StartNode
        {
            get => _startNode;
            set
            {
                if (_startNode != null) { _startNode.IsStartNode = false; }
                _startNode = value;
                if (_startNode != null) { _startNode.IsStartNode = true; }
                Notify();
            }
        }

        private DijkstraNodeViewModel? _endNode = null;
        public DijkstraNodeViewModel? EndNode
        {
            get => _endNode;
            set
            {
                if (_endNode != null) { _endNode.IsEndNode = false; }
                _endNode = value;
                if (_endNode != null) { _endNode.IsEndNode = true; }
                Notify();
            }
        }

        public DijkstraObjectViewCollection<Node, DijkstraNodeViewModel> _nodeViewCollection;
        public DijkstraObjectViewCollection<Edge, DijkstraEdgeViewModel> _edgeViewCollection;

        public ObservableCollection<DijkstraObjectViewModel> DijkstraViewObjects { get; private set; } = new();
        public ObservableCollection<DijkstraNodeViewModel> SelectedNodes { get; private set; } = new();

        public ICommand CreateEdgesCommand { get; set; }
        public ICommand DeleteNodesCommand { get; set; }
        public ICommand RunDijkstraAlgorithmCommand { get; set; }


        public GraphViewModel()
        {
            CreateEdgesCommand = new Commands.CreateEdgesCommand(this);
            DeleteNodesCommand = new Commands.DeleteNodesCommand(this);
            RunDijkstraAlgorithmCommand = new Commands.RunDijkstraAlgorithmCommand(this);
            _nodeViewCollection = new(_dijkstraGraph.Nodes, DijkstraNodeViewModel.MakeNodeViewModel);
            _nodeViewCollection.AddOrRemove += AddOrRemoveDijkstraNode;

            _edgeViewCollection = new(_dijkstraGraph.Edges, DijkstraEdgeViewModel.MakeEdgeViewModel);
            _edgeViewCollection.AddOrRemove += AddOrRemoveDijkstraEdge;
        }


        public void AddNewNode(double x, double y)
        {
            _dijkstraGraph.AddNode(x, y);
        }

        public void DeleteNode(DijkstraNodeViewModel node)
        {
            _dijkstraGraph.RemoveNode(node.Node);
        }

        public void DeleteSelectedNodes()
        {
            foreach (var node in SelectedNodes.ToList())
            {
                DeleteNode(node);
            }
        }       

        public void CreateEdge(DijkstraNodeViewModel node1, DijkstraNodeViewModel node2)
        {
            _dijkstraGraph.AddEdge(node1.Node, node2.Node);
        }

        public void CreateEdgesFromSelected()
        {
            for (int nodeIndex = 1; nodeIndex < SelectedNodes.Count; ++nodeIndex)
            {
                CreateEdge(SelectedNodes[nodeIndex - 1], SelectedNodes[nodeIndex]);
            }
        }

        private void AddOrRemoveDijkstraEdge(DijkstraObjectViewModel dijkstraObject, bool isAdd)
        {
            if (isAdd)
            {
                DijkstraViewObjects.Add(dijkstraObject);
            }
            else
            {
                DijkstraViewObjects.Remove(dijkstraObject);
            }
        }

        private void AddOrRemoveDijkstraNode(DijkstraObjectViewModel dijkstraObject, bool isAdd)
        {
            if (isAdd)
            {
                ((DijkstraNodeViewModel)dijkstraObject).UserInteraction += NodeUserInteractionHandler;
                DijkstraViewObjects.Add(dijkstraObject);
            }
            else
            {
                RemoveSelectedNode((DijkstraNodeViewModel)dijkstraObject);
                if (dijkstraObject == StartNode) { StartNode = null; }
                if (dijkstraObject == EndNode) { EndNode = null; }
                DijkstraViewObjects.Remove(dijkstraObject);
            }
        }

        public void ClearSelectedNodes()
        {
            foreach (var node in SelectedNodes)
            {
                node.IsSelected = false;
            }
            SelectedNodes.Clear();
        }

        public void AddSelectedNode(DijkstraNodeViewModel node)
        {
            if (SelectedNodes.Contains(node)) { return; }

            node.IsSelected = true;
            SelectedNodes.Add(node);
        }

        public void RemoveSelectedNode(DijkstraNodeViewModel node)
        {
            node.IsSelected = false;
            SelectedNodes.Remove(node);
        }

        public void ToggleSelectedNode(DijkstraNodeViewModel node)
        {
            if (node.IsSelected) { RemoveSelectedNode(node); }
            else { AddSelectedNode(node); }
        }

        private void NodeUserInteractionHandler(object? sender, UserInteractionEventArgs e)
        {
            if (!(sender is DijkstraNodeViewModel node)) { return; }

            switch (e.State)
            {
                case UserInteractionState.BeginDrag:
                    ResetAllDijkstraObjects();
                    break;
                case UserInteractionState.EndInteraction:
                    if (!node.WasMovedWhileInteracting)
                    {
                        ToggleSelectedNode(node);
                    }
                    break;
                case UserInteractionState.SetAsStart: StartNode = node; break;
                case UserInteractionState.SetAsEnd: EndNode = node; break;
                case UserInteractionState.Delete: DeleteNode(node); break;
            }
        }

        private void ResetAllDijkstraObjects()
        {
            foreach (var dijkstraObject in DijkstraViewObjects)
            {
                dijkstraObject.Reset();
            }
        }

        public void HighlightRoute(List<Node> nodes)
        {
            for (int nodeIndex = 0; nodeIndex < nodes.Count; nodeIndex++)
            {
                var currentNode = nodes[nodeIndex];

                var nodeViewModel = _nodeViewCollection.GetViewModel(currentNode);
                nodeViewModel.IsHighlighted = true;

                if (nodeIndex < nodes.Count - 1)
                {
                    var nextNode = nodes[nodeIndex + 1];
                    Edge? sharedEdge = currentNode.FindSharedEdge(nextNode);
                    if (sharedEdge == null) { throw new Exception("No Edge found between route nodes"); }

                    var edgeViewModel = _edgeViewCollection.GetViewModel(sharedEdge);
                    edgeViewModel.IsHighlighted = true;
                }

            }
        }

        public void UpdateDijkstraView(DijkstraState dijkstraState)
        {
            ResetAllDijkstraObjects();
            foreach (var node in dijkstraState.DijkstraNodes)
            {
                var nodeViewModel = _nodeViewCollection.GetViewModel(node.Node);
                nodeViewModel.RouteSegmentDistance = node.RouteSegmentDistance;
            }

            var shortestPathList = dijkstraState.GenerateShortestPathList();
            HighlightRoute(shortestPathList.Select(dijkstraNode => dijkstraNode.Node).ToList());
        }

        public void RunDijkstraAlgorithm()
        {
            if (StartNode == null || EndNode == null) { return; }
            try
            {
                var result = Dijkstra.FindShortestPath(StartNode.Node, EndNode.Node);
                UpdateDijkstraView(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
    }
}
