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

        public class DijkstraGraphViewModelState : IGraphState
        {
            private Node? _startNode;
            private Node? _endNode;
            public void RestoreState(BaseGraphViewModel viewModel)
            {
                var dijkstraViewModel = ((DijkstraGraphViewModel)viewModel);
                dijkstraViewModel.SetAsStartNode(_startNode);
                dijkstraViewModel.SetAsEndNode(_endNode);
            }

            public void StoreState(BaseGraphViewModel viewModel)
            {
                var dijkstraViewModel = ((DijkstraGraphViewModel)viewModel);
                _startNode = dijkstraViewModel.StartNode?.Node;
                _endNode = dijkstraViewModel.EndNode?.Node;
            }
        }

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

        protected override void AddOrRemoveNodeViewModel(GraphObjectViewModel dijkstraObject, bool isAdd)
        {
            if (!isAdd)
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
                if (IsNodeDragging && SelectedExecutionMode != AlgorithmExecutionModeEnum.Continuous) { return; }
                if (StartNode == null || EndNode == null)
                {
                    ResetAllDijkstraViewObjects();
                }
                else
                {
                    RunDijkstraAlgorithm();
                }
            }
            else
            {
                ResetAllDijkstraViewObjects();
            }
        }

        protected override void OnNodeBeginDrag(NodeViewModel node)
        {
            ResetAlgorithm();
            base.OnNodeBeginDrag(node);
        }

        protected override void NodeUserInteractionHandler(object? sender, UserInteractionEventArgs e)
        {
            base.NodeUserInteractionHandler(sender, e);

            if (!(sender is DijkstraNodeViewModel node)) { return; }

            switch (e.State)
            {
                case UserInteractionState.SetAsStart:
                    SetAsStartNode(node);
                    break;
                case UserInteractionState.SetAsEnd:
                    SetAsEndNode(node);
                    break;
            }
        }

        public void SetAsStartNode(DijkstraNodeViewModel node)
        {
            IGraphState previous = GetStateSnapshot();
            StartNode = node;
            UndoStack.AddItem(new GraphStateUndoItem(this, previous));
        }

        public void SetAsStartNode(Node? node)
        {
            if (node == null)
            {
                StartNode = null;
            }
            else if (HasNode(node))
            {
                StartNode = GetViewModel(node);
            }            
        }

        public void SetAsEndNode(DijkstraNodeViewModel node)
        {
            IGraphState previous = GetStateSnapshot();
            EndNode = node;
            UndoStack.AddItem(new GraphStateUndoItem(this, previous));
        }

        public void SetAsEndNode(Node? node)
        {
            if (node == null)
            {
                EndNode = null;
            }
            else if (HasNode(node))
            {
                EndNode = GetViewModel(node);
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
                if (nodeViewModel == null) { continue; }

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


        public override IGraphState GetStateSnapshot()
        {
            DijkstraGraphViewModelState state = new DijkstraGraphViewModelState();
            state.StoreState(this);
            return state;
        }
    }
}
