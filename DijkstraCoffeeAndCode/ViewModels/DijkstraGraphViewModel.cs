// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraAlgorithm.File;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Linq;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum AlgorithmExecutionModeEnum { Manual, OnEnd, Continuous };


    public class DijkstraGraphViewModel : BaseGraphViewModel
    {

        private NodeViewModel? _startNode = null;
        public NodeViewModel? StartNode
        {
            get => _startNode;
            set
            {
                if (_startNode != null) { _startNode.IsStartNode = false; }
                _startNode = value;
                if (_startNode != null)
                {
                    if (_endNode == _startNode) { EndNode = null; }
                    _startNode.IsStartNode = true;
                }
                Notify();
                OnGraphChanged();
            }
        }

        private NodeViewModel? _endNode = null;
        public NodeViewModel? EndNode
        {
            get => _endNode;
            set
            {
                if (_endNode != null) { _endNode.IsEndNode = false; }
                _endNode = value;
                if (_endNode != null)
                {
                    if (_endNode == _startNode) { StartNode = null; }
                    _endNode.IsEndNode = true;
                }
                Notify();
                OnGraphChanged();
            }
        }

        private DijkstraState? _dijkstraStepState = null;
        public DijkstraState? DijkstraStepState
        {
            get { return _dijkstraStepState; }
            set
            {
                _dijkstraStepState = value;
                Notify();
            }
        }


        private AlgorithmExecutionModeEnum _selectedExecutionMode = AlgorithmExecutionModeEnum.Manual;
        public AlgorithmExecutionModeEnum SelectedExecutionMode
        {
            get { return _selectedExecutionMode; }
            set
            {
                if (_selectedExecutionMode == value) { return; }
                _selectedExecutionMode = value;
                Notify();
                OnGraphChanged();
            }
        }

        public IEnumerable<AlgorithmExecutionModeEnum> AlgorithmExecutionModes
        {
            get => Enum.GetValues(typeof(AlgorithmExecutionModeEnum)).Cast<AlgorithmExecutionModeEnum>();
        }

        private int _shortestPathDistance = 0;
        public int ShortestPathDistance
        {
            get { return _shortestPathDistance; }
            set { _shortestPathDistance = value; Notify(); }
        }

        public ICommand ResetGraphCommand { get; set; }
        public ICommand RunDijkstraAlgorithmCommand { get; set; }
        public ICommand RunDijkstraStepCommand { get; set; }

        public DijkstraGraphViewModel() : base(DijkstraNodeViewModel.MakeNodeViewModel)
        {
            RunDijkstraAlgorithmCommand = new RunDijkstraAlgorithmCommand(this);
            RunDijkstraStepCommand = new RunDijkstraStepCommand(this);
            ResetGraphCommand = new ResetGraphCommand(this);
        }

        public override void Clear()
        {
            StartNode = null;
            EndNode = null;
            base.Clear();
        }

        public void DeleteNode(NodeViewModel node)
        {
            DeleteNode(node.Node);
        }

        public void DeleteEdge(NodeViewModel node1, NodeViewModel node2)
        {
            DeleteEdge(node1.Node, node2.Node);
        }

        public void CreateEdge(NodeViewModel node1, NodeViewModel node2)
        {
            CreateEdge(node1.Node, node2.Node);
        }

        protected override void AddOrRemoveEdgeViewModel(GraphObjectViewModel dijkstraObject, bool isAdd)
        {
            base.AddOrRemoveEdgeViewModel (dijkstraObject, isAdd);
        }

        protected override void AddOrRemoveNodeViewModel(GraphObjectViewModel dijkstraObject, bool isAdd)
        {
            if (isAdd)
            {
                ((NodeViewModel)dijkstraObject).UserInteraction += NodeUserInteractionHandler;
            }
            else
            {
                if (dijkstraObject == StartNode) { StartNode = null; }
                if (dijkstraObject == EndNode) { EndNode = null; }
            }
            base.AddOrRemoveNodeViewModel(dijkstraObject, isAdd);
        }

        protected override void OnGraphChanged()
        {
            if (SelectedExecutionMode != AlgorithmExecutionModeEnum.Manual)
            {
                if (StartNode == null || EndNode == null)
                {
                    ResetAllDijkstraViewObjects();
                }
                else
                {
                    RunDijkstraAlgorithm();
                }
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

        private void ProcessUserDragNode(NodeViewModel node, UserInteractionEventArgs e)
        {
            if (SelectedExecutionMode == AlgorithmExecutionModeEnum.Continuous)
            {
                OnGraphChanged();
            }
            if (!IsMultiSelectMode()) { return; }
            if (e.Data == null) { return; }
            if (!(e.Data is Vector2D positionDelta)) { return; }

            MoveOtherSelectedNodes(node, positionDelta.X, positionDelta.Y);
        }

        private void NodeUserInteractionHandler(object? sender, UserInteractionEventArgs e)
        {
            if (!(sender is NodeViewModel node)) { return; }

            switch (e.State)
            {
                case UserInteractionState.BeginDrag:
                    ResetAlgorithm();
                    break;
                case UserInteractionState.EndDrag:
                    if (SelectedExecutionMode == AlgorithmExecutionModeEnum.OnEnd)
                    {
                        OnGraphChanged();
                    }
                    break;
                case UserInteractionState.ContinueDrag:
                    ProcessUserDragNode(node, e);
                    break;
                case UserInteractionState.EndInteraction:
                    if (!node.WasMovedWhileInteracting)
                    {
                        ToggleSelectedNode(node, IsMultiSelectMode());
                    }
                    break;
                case UserInteractionState.SetAsStart: StartNode = node; break;
                case UserInteractionState.SetAsEnd: EndNode = node; break;
                case UserInteractionState.Delete: DeleteNode(node); break;
            }
        }

        private void ResetAlgorithm()
        {
            DijkstraStepState = null;
            ResetAllDijkstraViewObjects();
        }

        public void ResetAllDijkstraViewObjects()
        {
            foreach (var dijkstraObject in GraphViewObjects)
            {
                dijkstraObject.Reset();
            }
        }

        public void HighlightRoute(List<Node> nodes)
        {
            for (int nodeIndex = 0; nodeIndex < nodes.Count; nodeIndex++)
            {
                var currentNode = nodes[nodeIndex];

                var nodeViewModel = GetViewModel(currentNode);
                nodeViewModel.IsHighlighted = true;

                if (nodeIndex < nodes.Count - 1)
                {
                    var nextNode = nodes[nodeIndex + 1];
                    Edge? sharedEdge = GetEdge(currentNode, nextNode);
                    if (sharedEdge == null) { throw new Exception("No Edge found between route nodes"); }

                    var edgeViewModel = GetViewModel(sharedEdge);
                    edgeViewModel.IsHighlighted = true;
                }

            }
        }

        public void UpdateDijkstraView(DijkstraState dijkstraState)
        {
            ResetAllDijkstraViewObjects();
            foreach (var node in dijkstraState.DijkstraNodes)
            {
                var nodeViewModel = GetViewModel(node.Node) as DijkstraNodeViewModel;
                if(nodeViewModel == null) { continue; }

                nodeViewModel.RouteSegmentDistance = node.RouteSegmentDistance;
                if (!dijkstraState.IsFinished)
                {
                    nodeViewModel.IsVisited = node.IsVisited;
                }
            }

            if (dijkstraState.IsFinished)
            {
                var shortestPathList = dijkstraState.GenerateShortestPathList();
                ShortestPathDistance = (int)Math.Round(dijkstraState.EndNode.RouteSegmentDistance);
                HighlightRoute(shortestPathList.Select(dijkstraNode => dijkstraNode.Node).ToList());
            }
            else
            {
                ShortestPathDistance = 0;
                if (dijkstraState.CurrentNode == null)
                {
                    throw new Exception("Invalid Dijkstra State");
                }
                var nodeViewModel = GetViewModel(dijkstraState.CurrentNode.Node);
                nodeViewModel.IsHighlighted = true;

                if (dijkstraState.LastCheckedNeighbor != null)
                {
                    nodeViewModel = GetViewModel(dijkstraState.LastCheckedNeighbor.Node);
                    nodeViewModel.IsHighlightedAlternate = true;
                }


            }
        }

        public void RunDijkstraStepAlgorithm()
        {
            if (StartNode == null || EndNode == null) { return; }
            try
            {
                if (DijkstraStepState == null || DijkstraStepState.IsFinished)
                {
                    DijkstraStepState = Dijkstra.TakeStep(StartNode.Node, EndNode.Node);
                }
                else
                {
                    Dijkstra.TakeStep(DijkstraStepState);
                }

                UpdateDijkstraView(DijkstraStepState);
            }
            catch (Exception e)
            {
                RaiseMessage("Could not run algorithm on current graph");
                Debug.WriteLine(e.ToString());
            }
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
                RaiseMessage("Could Not Run Algorithm On Current Graph");
                // Set to manual mode to prevent slamming the algorithm with invalid graphs.
                if (SelectedExecutionMode != AlgorithmExecutionModeEnum.Manual)
                {
                    SelectedExecutionMode = AlgorithmExecutionModeEnum.Manual;
                }
                Debug.WriteLine(e.ToString());
            }

        }

    }
}
