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
        private Graph _dijkstraGraph = new Graph();
        public ObservableCollection<DijkstraObjectViewModel> DijkstraObjects { get; private set; } = new();
        private Dictionary<Node, DijkstraNodeViewModel> _nodesToViewModel = new();
        private Dictionary<Edge, DijkstraEdgeViewModel> _edgesToViewModel = new();

        public ObservableCollection<DijkstraNodeViewModel> SelectedNodes { get; private set; } = new();
        public const int MAX_SELECTED_NODES = 2;

        public ICommand CreateEdgesCommand { get; set; }
        public ICommand DeleteNodesCommand { get; set; }
        public ICommand RunDijkstraAlgorithmCommand { get; set; }

        private DijkstraNodeViewModel? _startNode = null;
        private DijkstraNodeViewModel? _endNode = null;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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

        public GraphViewModel()
        {
            CreateEdgesCommand = new Commands.CreateEdgesCommand(this);
            DeleteNodesCommand = new Commands.DeleteNodesCommand(this);
            RunDijkstraAlgorithmCommand = new Commands.RunDijkstraAlgorithmCommand(this);

            _dijkstraGraph.Nodes.CollectionChanged += GraphNodesCollectionChanged;
            _dijkstraGraph.Edges.CollectionChanged += GraphEdgesCollectionChanged;
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

        private void GraphNodesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems == null || e.NewItems.Count == 0) { return; }
                        if (!(e.NewItems[0] is Node node)) { return; }
                        AddNewNodeViewModel(node);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems == null || e.OldItems.Count == 0) { return; }
                        if (!(e.OldItems[0] is Node node)) { return; }
                        RemoveNodeViewModel(node);
                    }
                    break;
            }
        }

        private DijkstraNodeViewModel GetDijkstraNodeViewModel(Node node)
        {
            if (_nodesToViewModel.ContainsKey(node))
            {
                return _nodesToViewModel[node];
            }

            throw new Exception("Node not found in collection.");
        }

        private void AddNewNodeViewModel(Node node)
        {
            DijkstraNodeViewModel nodeViewModel = new(node);
            nodeViewModel.UserInteraction += NodeUserInteractionHandler;
            _nodesToViewModel.Add(node, nodeViewModel);
            DijkstraObjects.Add(nodeViewModel);
        }

        private void RemoveNodeViewModel(Node node)
        {
            DijkstraNodeViewModel nodeToRemove = GetDijkstraNodeViewModel(node);
            RemoveSelectedNode(nodeToRemove);
            if (nodeToRemove == StartNode) { StartNode = null; }
            if (nodeToRemove == EndNode) { EndNode = null; }

            _nodesToViewModel.Remove(node);
            DijkstraObjects.Remove(nodeToRemove);
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

        private void GraphEdgesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems == null || e.NewItems.Count == 0) { return; }
                        if (!(e.NewItems[0] is Edge edge)) { return; }
                        AddEdgeViewModel(edge);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems == null || e.OldItems.Count == 0) { return; }
                        if (!(e.OldItems[0] is Edge edge)) { return; }
                        RemoveEdgeViewModel(edge);
                    }
                    break;
            }
        }

        private DijkstraEdgeViewModel GetEdgeViewModel(Edge edge)
        {
            if (_edgesToViewModel.ContainsKey(edge))
            {
                return _edgesToViewModel[edge];
            }

            throw new Exception("Edge not found in collection.");
        }

        private void AddEdgeViewModel(Edge edge)
        {
            DijkstraEdgeViewModel edgeViewModel = new(edge);
            _edgesToViewModel.Add(edge, edgeViewModel);
            DijkstraObjects.Add(edgeViewModel);
        }

        private void RemoveEdgeViewModel(Edge edge)
        {
            DijkstraEdgeViewModel edgeToRemove = GetEdgeViewModel(edge);
            _edgesToViewModel.Remove(edge);
            DijkstraObjects.Remove(edgeToRemove);
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
            foreach (var dijkstraObject in DijkstraObjects)
            {
                dijkstraObject.Reset();
            }
        }

        public void HighlightRoute(List<Node> nodes)
        {
            for (int nodeIndex = 0; nodeIndex < nodes.Count; nodeIndex++)
            {
                var currentNode = nodes[nodeIndex];

                var nodeViewModel = GetDijkstraNodeViewModel(currentNode);
                nodeViewModel.IsHighlighted = true;

                if (nodeIndex < nodes.Count - 1)
                {
                    var nextNode = nodes[nodeIndex + 1];
                    Edge? sharedEdge = currentNode.FindSharedEdge(nextNode);
                    if (sharedEdge == null) { throw new Exception("No Edge found between route nodes"); }

                    var edgeViewModel = GetEdgeViewModel(sharedEdge);
                    edgeViewModel.IsHighlighted = true;
                }

            }
        }

        public void UpdateDijkstraView(DijkstraState dijkstraState)
        {
            ResetAllDijkstraObjects();
            foreach (var node in dijkstraState.DijkstraNodes)
            {
                var nodeViewModel = GetDijkstraNodeViewModel(node.Node);
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
