// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraAlgorithm.File;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum AlgorithmExecutionModeEnum { Manual, OnEnd, Continuous };
    public delegate string GetFilePath(bool isOpen, string fileExtensions);
    public delegate void MessageEvent(string message);
    public class GraphViewModel : INotifyPropertyChanged
    {
        public const int MAX_SELECTED_NODES = 2;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private DijkstraNodeViewModel? _startNode = null;
        public DijkstraNodeViewModel? StartNode
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

        private DijkstraNodeViewModel? _endNode = null;
        public DijkstraNodeViewModel? EndNode
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

        private Graph _dijkstraGraph;
        private DijkstraObjectViewCollection<Node, DijkstraNodeViewModel> _nodeViewCollection;
        private DijkstraObjectViewCollection<Edge, DijkstraEdgeViewModel> _edgeViewCollection;

        public ObservableCollection<DijkstraObjectViewModel> DijkstraViewObjects { get; private set; } = new();
        public ObservableCollection<DijkstraNodeViewModel> SelectedNodes { get; private set; } = new();

        public ICommand CreateEdgesCommand { get; set; }
        public ICommand DeleteSelectedNodesCommand { get; set; }
        public ICommand DeleteSelectedEdgesCommand { get; set; }
        public ICommand DeleteAllNodesCommand { get; set; }
        public ICommand DeleteAllEdgesCommand { get; set; }
        public ICommand RunDijkstraAlgorithmCommand { get; set; }
        public ICommand RunDijkstraStepCommand { get; set; }
        public ICommand SaveGraphCommand { get; set; }
        public ICommand LoadGraphCommand { get; set; }
        public ICommand ImportGraphCommand { get; set; }
        public ICommand NewGraphCommand { get; set; }

        public GetFilePath? GetFilePath { get; set; }
        public event MessageEvent MessageEvent;

        private string _currentFilePath = "";

        public string CurrentFilePath
        {
            get { return _currentFilePath; }
            set
            {
                _currentFilePath = value;
                Notify();
                Notify(nameof(CurrentFileName));
            }
        }

        public string CurrentFileName
        {
            get
            {
                if (String.IsNullOrEmpty(_currentFilePath)) { return "New Graph"; }
                return Path.GetFileNameWithoutExtension(_currentFilePath);
            }
        }

        public GraphViewModel()
        {
            CreateEdgesCommand = new CreateEdgesCommand(this);
            DeleteSelectedNodesCommand = new DeleteSelectedNodesCommand(this);
            DeleteSelectedEdgesCommand = new DeleteSelectedEdgesCommand(this);
            DeleteAllNodesCommand = new DeleteAllNodesCommand(this);
            DeleteAllEdgesCommand = new DeleteAllEdgesCommand(this);
            RunDijkstraAlgorithmCommand = new RunDijkstraAlgorithmCommand(this);
            RunDijkstraStepCommand = new RunDijkstraStepCommand(this);
            SaveGraphCommand = new SaveGraphCommand(this);
            LoadGraphCommand = new LoadGraphCommand(this);
            ImportGraphCommand = new ImportGraphCommand(this);
            NewGraphCommand = new NewGraphCommand(this);

            SetGraph(new Graph());
        }

        private void Clear()
        {
            StartNode = null;
            EndNode = null;
            DijkstraViewObjects.Clear();
            SelectedNodes.Clear();
        }

        private void SetGraph(Graph graph)
        {
            Clear();
            _dijkstraGraph = graph;
            _nodeViewCollection = new(_dijkstraGraph.Nodes, DijkstraNodeViewModel.MakeNodeViewModel);
            _nodeViewCollection.AddOrRemove += AddOrRemoveDijkstraNode;

            foreach (var nodeView in _nodeViewCollection.Values)
            {
                AddOrRemoveDijkstraNode(nodeView, true);
            }

            _edgeViewCollection = new(_dijkstraGraph.Edges, DijkstraEdgeViewModel.MakeEdgeViewModel);
            _edgeViewCollection.AddOrRemove += AddOrRemoveDijkstraEdge;

            foreach (var edgeView in _edgeViewCollection.Values)
            {
                AddOrRemoveDijkstraEdge(edgeView, true);
            }
        }

        public void AddNewNode(double x, double y)
        {
            _dijkstraGraph.AddNode(x, y);
        }

        public void DeleteNode(DijkstraNodeViewModel node)
        {
            _dijkstraGraph.RemoveNode(node.Node);
        }

        public void DeleteAllNodes()
        {
            _dijkstraGraph.RemoveAllNodes();
        }

        public void DeleteSelectedNodes()
        {
            foreach (var node in SelectedNodes.ToList())
            {
                DeleteNode(node);
            }
            ClearSelectedNodes();
        }

        public void DeleteEdge(DijkstraNodeViewModel node1, DijkstraNodeViewModel node2)
        {
            _dijkstraGraph.RemoveEdge(node1.Node, node2.Node);
        }

        public void DeleteAllEdges()
        {
            _dijkstraGraph.RemoveAllEdges();
        }

        public void DeleteSelectedEdges()
        {
            if (SelectedNodes.Count < 2) { return; }

            for (int nodeIndex = 1; nodeIndex < SelectedNodes.Count; ++nodeIndex)
            {
                DeleteEdge(SelectedNodes[nodeIndex - 1], SelectedNodes[nodeIndex]);
            }

            DeleteEdge(SelectedNodes[SelectedNodes.Count - 1], SelectedNodes[0]);
            ClearSelectedNodes();
        }

        public void CreateEdge(DijkstraNodeViewModel node1, DijkstraNodeViewModel node2)
        {
            _dijkstraGraph.AddEdge(node1.Node, node2.Node);
        }

        public void CreateEdgesFromSelected()
        {
            if (SelectedNodes.Count < 2) { return; }

            // order of node selection affects which edges are deleted.
            // alternatively can check all nodes against all other nodes.

            for (int nodeIndex = 1; nodeIndex < SelectedNodes.Count; ++nodeIndex)
            {
                CreateEdge(SelectedNodes[nodeIndex - 1], SelectedNodes[nodeIndex]);
            }

            CreateEdge(SelectedNodes[SelectedNodes.Count - 1], SelectedNodes[0]);
            ClearSelectedNodes();
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
            OnGraphChanged();
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
            OnGraphChanged();
        }

        private void OnGraphChanged()
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
                    ResetAlgorithm();
                    break;
                case UserInteractionState.EndDrag:
                    OnGraphChanged();
                    break;
                case UserInteractionState.ContinueDrag:
                    OnGraphChanged();
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

        private void ResetAlgorithm()
        {
            DijkstraStepState = null;
            ResetAllDijkstraViewObjects();
        }

        private void ResetAllDijkstraViewObjects()
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
            ResetAllDijkstraViewObjects();
            foreach (var node in dijkstraState.DijkstraNodes)
            {
                var nodeViewModel = _nodeViewCollection.GetViewModel(node.Node);
                nodeViewModel.RouteSegmentDistance = node.RouteSegmentDistance;
                if (!dijkstraState.IsFinished)
                {
                    nodeViewModel.IsVisited = node.IsVisited;
                }
            }

            if (dijkstraState.IsFinished)
            {
                var shortestPathList = dijkstraState.GenerateShortestPathList();
                HighlightRoute(shortestPathList.Select(dijkstraNode => dijkstraNode.Node).ToList());
            }
            else
            {
                if (dijkstraState.CurrentNode == null)
                {
                    throw new Exception("Invalud Dijkstra State");
                }
                var nodeViewModel = _nodeViewCollection.GetViewModel(dijkstraState.CurrentNode.Node);
                nodeViewModel.IsHighlighted = true;

                if (dijkstraState.LastCheckedNeighbor != null)
                {
                    nodeViewModel = _nodeViewCollection.GetViewModel(dijkstraState.LastCheckedNeighbor.Node);
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
                MessageEvent?.Invoke("Could not run algorithm on current graph");
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
                MessageEvent?.Invoke("Could not run algorithm on current graph");
                // Set to manual mode to prevent slamming the algorithm with invalid graphs.
                if (SelectedExecutionMode != AlgorithmExecutionModeEnum.Manual)
                {
                    SelectedExecutionMode = AlgorithmExecutionModeEnum.Manual;
                }
                Debug.WriteLine(e.ToString());
            }

        }

        public void LoadGraph()
        {
            if (GetFilePath == null) { return; }
            try
            {
                string filePath = GetFilePath(true, GraphFile.FILE_FILTER);
                if (String.IsNullOrEmpty(filePath)) { return; }
                SetGraph(Graph.LoadGraph(filePath));
                CurrentFilePath = filePath;
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Load Graph");
                Debug.WriteLine(e.Message);
            }
        }

        public void ImportGraph()
        {
            if (GetFilePath == null) { return; }
            try
            {
                string filePath = GetFilePath(true, GraphFile.FILE_FILTER);
                if (String.IsNullOrEmpty(filePath)) { return; }
                _dijkstraGraph.ImportGraph(filePath);
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Import Graph");
                Debug.WriteLine(e.Message);
            }

        }

        public void NewGraph()
        {
            CurrentFilePath = "";
            SetGraph(new Graph());
        }

        public void SaveGraph(bool isSaveAs = true)
        {
            if (GetFilePath == null) { return; }
            try
            {
                if (isSaveAs || String.IsNullOrEmpty(CurrentFilePath))
                {
                    string filePath = GetFilePath(false, GraphFile.FILE_FILTER);
                    if (String.IsNullOrEmpty(filePath)) { return; }
                    Graph.SaveGraph(_dijkstraGraph, filePath);
                    CurrentFilePath = filePath;
                }
                else
                {
                    Graph.SaveGraph(_dijkstraGraph, CurrentFilePath);
                }

            }
            catch (Exception e)
            {
                MessageEvent?.Invoke("Could Not Save Graph");
                Debug.WriteLine(e.Message);
            }
        }


    }
}
