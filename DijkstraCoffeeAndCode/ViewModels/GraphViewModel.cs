using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
            RemoveSelectedNode(node);
            if (node == StartNode) { StartNode = null; }
            if (node == EndNode) { EndNode = null; }
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

        private void AddNewNodeViewModel(Node node)
        {
            DijkstraNodeViewModel nodeViewModel = new(node);
            nodeViewModel.UserInteraction += NodeUserInteractionHandler;
            DijkstraObjects.Add(nodeViewModel);
        }

        private void RemoveNodeViewModel(Node node)
        {
            DijkstraNodeViewModel? nodeToRemove = DijkstraObjects.FirstOrDefault(dijkstraObject =>
            {
                if (dijkstraObject is DijkstraNodeViewModel dijkstraNode)
                {
                    return dijkstraNode.Node == node;
                }
                return false;
            }) as DijkstraNodeViewModel;

            if (nodeToRemove == null) { return; }

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

        private void AddEdgeViewModel(Edge edge)
        {
            DijkstraObjects.Add(new DijkstraEdgeViewModel(edge));
        }

        private void RemoveEdgeViewModel(Edge edge)
        {
            DijkstraEdgeViewModel? edgeToRemove = DijkstraObjects.FirstOrDefault(dijkstraObject =>
            {
                if (dijkstraObject is DijkstraEdgeViewModel dijkstraEdge)
                {
                    return dijkstraEdge.Edge == edge;
                }
                return false;
            }) as DijkstraEdgeViewModel;

            if (edgeToRemove == null) { return; }

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
                case UserInteractionState.EndDrag:
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


        public void RunDijkstraAlgorithm()
        {
            if (StartNode == null || EndNode == null) { return; }

            try
            {
                foreach (var node in Dijkstra.FindShortestPath(StartNode.Node, EndNode.Node))
                {
                    Debug.WriteLine(node.ShortestRouteDistance);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
    }
}
