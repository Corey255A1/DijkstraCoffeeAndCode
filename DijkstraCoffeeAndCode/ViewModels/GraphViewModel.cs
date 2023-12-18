using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class GraphViewModel
    {
        public ObservableCollection<DijkstraObjectViewModel> DijkstraObjects { get; private set; } = new();

        public ObservableCollection<DijkstraNodeViewModel> SelectedNodes { get; private set; } = new();
        public const int MAX_SELECTED_NODES = 2;

        public ICommand CreateEdges { get; set; }
        public ICommand DeleteNodes { get; set; }

        public GraphViewModel() {
            CreateEdges = new Commands.CreateEdgesCommand(this);
            DeleteNodes = new Commands.DeleteNodesCommand(this);
        }

        public void AddNewNode(double x, double y)
        {
            DijkstraNodeViewModel node = new(x, y);
            node.UserInteraction += NodeUserInteractionHandler;
            node.EdgeEvent += NodeEdgeEvent;
            DijkstraObjects.Add(node);
        }

        private void NodeEdgeEvent(DijkstraAlgorithm.Edge edge, DijkstraAlgorithm.EdgeAction action)
        {
            switch(action) {
                case DijkstraAlgorithm.EdgeAction.Created: AddEdge(edge); break;
                case DijkstraAlgorithm.EdgeAction.Deleted: RemoveEdge(edge); break;
            }
        }

        public void AddEdge(DijkstraAlgorithm.Edge edge)
        {
            int existingEdgeCount = DijkstraObjects
                    .Where(dijkstraObject => dijkstraObject as DijkstraEdgeViewModel != null)
                    .Cast<DijkstraEdgeViewModel>()
                    .Count(dijkstraObject => dijkstraObject.Edge == edge);
            if(existingEdgeCount > 0) { return; }

            DijkstraObjects.Add(new DijkstraEdgeViewModel(edge));
        }

        public void RemoveEdge(DijkstraAlgorithm.Edge edge)
        {
            var edgesToRemove = DijkstraObjects
                    .Where((dijkstraObject) =>
                    {
                        if (dijkstraObject is DijkstraEdgeViewModel dijkstraEdge) { 
                            return dijkstraEdge.Edge == edge; 
                        }
                        return false;
                    })
                    .Cast<DijkstraEdgeViewModel>().ToList();

            edgesToRemove.ForEach(dijkstraObject => DijkstraObjects.Remove(dijkstraObject));
        }

        public void CreateEdge(DijkstraNodeViewModel node1, DijkstraNodeViewModel node2)
        {
            node1.AddEdge(node2);            
        }

        public void CreateEdgesFromSelected()
        {
            for (int nodeIndex = 1; nodeIndex < SelectedNodes.Count; ++nodeIndex)
            {
                CreateEdge(SelectedNodes[nodeIndex - 1], SelectedNodes[nodeIndex]);
            }
        }

        public void ClearSelectedNodes()
        {
            foreach(var node in SelectedNodes)
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

        public void DeleteSelectedNodes()
        {
            foreach (var node in SelectedNodes) {
                node.RemoveAllEdges();
                DijkstraObjects.Remove(node);
            }
            ClearSelectedNodes();

        }

        private void NodeUserInteractionHandler(object? sender, UserInteractionEventArgs e)
        {
            if (!(sender is DijkstraNodeViewModel node)) { return; }

            if (e.State == UserInteractionState.End)
            {
                if (!node.WasMovedWhileInteracting)
                {
                    ToggleSelectedNode(node);
                }
            }
        }
    }
}
