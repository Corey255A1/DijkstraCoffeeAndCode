using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public delegate void MessageEventHandler(string message);
    public class BaseGraphViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event MessageEventHandler MessageEvent;

        private Graph _graph;
        public Graph Graph => _graph;

        private UndoStack _undoStack = new UndoStack();
        protected UndoStack UndoStack { get { return _undoStack; } }

        private GraphViewSelectionManager _selectionManager;
        public IEnumerable<Node> SelectedNodes => _selectionManager.SelectedNodes;
        public int SelectedNodesCount => _selectionManager.SelectedNodesCount;
        public NodeViewModel? SelectedNode => _selectionManager.SelectedNode;
        public event NotifyCollectionChangedEventHandler SelectedNodesChanged
        {
            add { _selectionManager.SelectedNodesChanged += value; }
            remove { _selectionManager.SelectedNodesChanged -= value; }
        }

        protected NodeViewModel? _currentDragNode;
        public bool IsNodeDragging => _currentDragNode != null;
        private NodePositionUndoItem? _currentNodePositionUndo;

        public ICommand CreateEdgesCommand { get; set; }
        public ICommand DeleteSelectedEdgesCommand { get; set; }
        public ICommand DeleteAllEdgesCommand { get; set; }

        public ICommand MakeNewNodeCommand { get; set; }
        public ICommand DeleteSelectedNodesCommand { get; set; }
        public ICommand DeleteAllNodesCommand { get; set; }

        public ICommand UndoCommand { get => UndoStack.UndoCommand; }
        public ICommand RedoCommand { get => UndoStack.RedoCommand; }

        private GraphObjectViewCollection<Node, NodeViewModel> _nodeViewCollection;
        public IEnumerable<Node> Nodes => _nodeViewCollection.Keys;

        private GraphObjectViewCollection<Edge, EdgeViewModel> _edgeViewCollection;

        public ObservableCollection<GraphObjectViewModel> GraphViewObjects { get; private set; } = new();

        private GraphObjectViewCollection<Node, NodeViewModel>.MakeViewModelFactory _nodeViewModelFactory;

        public BaseGraphViewModel(GraphObjectViewCollection<Node, NodeViewModel>.MakeViewModelFactory nodeViewModelFactory)
        {
            _nodeViewModelFactory = nodeViewModelFactory;
            _selectionManager = new GraphViewSelectionManager(this);

            CreateEdgesCommand = new CreateEdgesCommand(this, UndoStack);
            DeleteSelectedEdgesCommand = new DeleteSelectedEdgesCommand(this, UndoStack);
            DeleteAllEdgesCommand = new DeleteAllEdgesCommand(this, UndoStack);

            MakeNewNodeCommand = new MakeNewNodeCommand(this, UndoStack);
            DeleteSelectedNodesCommand = new DeleteSelectedNodesCommand(this, UndoStack);
            DeleteAllNodesCommand = new DeleteAllNodesCommand(this, UndoStack);

            SetGraph(new Graph());
        }

        public void RaiseMessage(string message)
        {
            MessageEvent?.Invoke(message);
        }

        public virtual void Clear()
        {
            GraphViewObjects.Clear();
            UndoStack.Clear();
            ClearSelectedNodes();
        }

        public void SetGraph(Graph graph)
        {
            Clear();
            _graph = graph;
            _nodeViewCollection = new(_graph.Nodes, _nodeViewModelFactory);
            _nodeViewCollection.AddOrRemove += AddOrRemoveNodeViewModel;

            foreach (var nodeView in _nodeViewCollection.Values)
            {
                AddOrRemoveNodeViewModel(nodeView, true);
            }

            _edgeViewCollection = new(_graph.Edges, EdgeViewModel.MakeEdgeViewModel);
            _edgeViewCollection.AddOrRemove += AddOrRemoveEdgeViewModel;

            foreach (var edgeView in _edgeViewCollection.Values)
            {
                AddOrRemoveEdgeViewModel(edgeView, true);
            }
        }

        protected virtual void AddOrRemoveEdgeViewModel(GraphObjectViewModel dijkstraObject, bool isAdd)
        {
            if (isAdd)
            {
                GraphViewObjects.Add(dijkstraObject);
            }
            else
            {
                GraphViewObjects.Remove(dijkstraObject);
            }
            OnGraphChanged();
        }

        protected virtual void AddOrRemoveNodeViewModel(GraphObjectViewModel dijkstraObject, bool isAdd)
        {
            if (isAdd)
            {
                GraphViewObjects.Add(dijkstraObject);
            }
            else
            {
                UnselectNode((NodeViewModel)dijkstraObject);
                GraphViewObjects.Remove(dijkstraObject);
            }
            OnGraphChanged();
        }

        protected virtual void OnGraphChanged() { }

        public Node MakeNewNode(double x, double y)
        {
            return _graph.AddNode(x, y);
        }

        public void AddNode(Node node)
        {
            _graph.AddNode(node);
        }

        public void SetNodePosition(Node node, Vector2D position)
        {
            GetViewModel(node).SetCenterPosition(position.X, position.Y);
        }

        public void DeleteNode(Node node)
        {
            _graph.RemoveNode(node);
        }

        public void DeleteNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                DeleteNode(node);
            }
            ClearSelectedNodes();
        }

        public void DeleteSelectedNodes()
        {
            DeleteNodes(_selectionManager.SelectedNodes.ToList());
        }

        public void DeleteAllNodes()
        {
            _graph.RemoveAllNodes();
        }

        public void CreateEdge(Node node1, Node node2)
        {
            _graph.AddEdge(node1, node2);
        }

        public void CreateEdges(List<Node> nodes)
        {
            if (nodes.Count < 2) { return; }

            int total = nodes.Count;
            // order of node selection affects which edges are deleted.
            // alternatively can check all nodes against all other nodes.

            for (int nodeIndex = 1; nodeIndex < total; ++nodeIndex)
            {
                CreateEdge(nodes[nodeIndex - 1], nodes[nodeIndex]);
            }

            CreateEdge(nodes[total - 1], nodes[0]);
        }

        public void CreateEdgesFromSelected()
        {
            CreateEdges(_selectionManager.SelectedNodes.ToList());
        }

        public void DeleteEdge(Node node1, Node node2)
        {
            _graph.RemoveEdge(node1, node2);
        }

        public void DeleteEdges(List<Node> nodes)
        {
            if (nodes.Count < 2) { return; }
            int total = nodes.Count;

            for (int nodeIndex = 1; nodeIndex < total; ++nodeIndex)
            {
                DeleteEdge(nodes[nodeIndex - 1], nodes[nodeIndex]);
            }

            DeleteEdge(nodes[total - 1], nodes[0]);
        }

        public void DeleteSelectedEdges()
        {
            DeleteEdges(_selectionManager.SelectedNodes.ToList());
        }

        public void DeleteAllEdges()
        {
            _graph.RemoveAllEdges();
        }

        public Edge? GetEdge(Node node1, Node node2)
        {
            return _edgeViewCollection.Keys.FirstOrDefault(edge => edge.Contains(node1, node2));
        }

        public NodeViewModel GetViewModel(Node node)
        {
            return _nodeViewCollection.GetViewModel(node);
        }

        public EdgeViewModel GetViewModel(Edge edge)
        {
            return _edgeViewCollection.GetViewModel(edge);
        }

        public void ClearSelectedNodes()
        {
            _selectionManager.Clear();
            Notify(nameof(SelectedNode));
        }

        public void SelectNodes(IEnumerable<NodeViewModel> nodes)
        {
            foreach (var node in nodes)
            {
                SelectNode(node, true);
            }
        }

        public void SelectNodes(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                SelectNode(GetViewModel(node), true);
            }
        }

        public void SelectNode(NodeViewModel node, bool isMultiSelect = false)
        {
            _selectionManager.Select(node, isMultiSelect);
            Notify(nameof(SelectedNode));
        }

        public void UnselectNode(NodeViewModel node)
        {
            _selectionManager.Unselect(node);
            Notify(nameof(SelectedNode));
        }

        public void ToggleSelectedNode(NodeViewModel node, bool isMultiSelect = false)
        {
            if (node.IsSelected)
            {
                if (SelectedNodesCount > 0 && !isMultiSelect)
                {
                    SelectNode(node, false);
                }
                else
                {
                    UnselectNode(node);
                }
            }
            else
            {
                SelectNode(node, isMultiSelect);
            }
        }

        private bool IsMultiSelectMode()
        {
            return Keyboard.GetKeyStates(Key.LeftShift).HasFlag(KeyStates.Down) ||
                Keyboard.GetKeyStates(Key.RightShift).HasFlag(KeyStates.Down);

        }

        private void MoveOtherSelectedNodes(NodeViewModel dragNode, double dX, double dY)
        {
            foreach (var selectedNode in SelectedNodes)
            {
                if (selectedNode != dragNode.Node)
                {
                    GetViewModel(selectedNode).Move(dX, dY);
                }
            }
        }

        protected virtual void OnNodeContinueDrag(NodeViewModel node, UserInteractionEventArgs e)
        {
            if (IsMultiSelectMode())
            {
                if (e.Data == null) { return; }
                if (!(e.Data is Vector2D positionDelta)) { return; }

                MoveOtherSelectedNodes(node, positionDelta.X, positionDelta.Y);
            }

            OnGraphChanged();
        }

        protected virtual void OnNodeBeginDrag(NodeViewModel node)
        {
            _currentDragNode = node;
            _currentNodePositionUndo = new(this);
        }

        protected virtual void OnNodeEndDrag(NodeViewModel node)
        {
            _currentDragNode = null;
            if (_currentNodePositionUndo != null)
            {
                _currentNodePositionUndo.SetFinalPosition();
                UndoStack.AddItem(_currentNodePositionUndo);
                _currentNodePositionUndo = null;
            }

            OnGraphChanged();
        }

        protected virtual void OnNodeBeginInteraction(NodeViewModel node) { }

        protected virtual void OnNodeEndInteraction(NodeViewModel node)
        {
            if (!node.WasMovedWhileInteracting)
            {
                ToggleSelectedNode(node, IsMultiSelectMode());
            }
        }

        protected virtual void OnNodeDelete(NodeViewModel node)
        {
            UndoStack.AddItem(new NodeDeleteUndoItem(this, node));
            DeleteNode(node.Node);
            
        }

        protected virtual void NodeUserInteractionHandler(object? sender, UserInteractionEventArgs e)
        {
            if (!(sender is NodeViewModel node)) { return; }

            switch (e.State)
            {
                case UserInteractionState.BeginInteraction:
                    OnNodeBeginInteraction(node);
                    break;
                case UserInteractionState.BeginDrag:
                    OnNodeBeginDrag(node);
                    break;
                case UserInteractionState.EndDrag:
                    OnNodeEndDrag(node);
                    break;
                case UserInteractionState.ContinueDrag:
                    OnNodeContinueDrag(node, e);
                    break;
                case UserInteractionState.EndInteraction:
                    OnNodeEndInteraction(node);
                    break;
                case UserInteractionState.Delete:
                    OnNodeDelete(node);
                    break;
            }
        }



    }
}
