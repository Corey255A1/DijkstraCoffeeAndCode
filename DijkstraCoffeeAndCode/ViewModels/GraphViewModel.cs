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

        //ICommand AddNodeCommand { get; set; }
        //ICommand UpdateNodeCommand { get; set; }


        public GraphViewModel() { }

        public void AddNewNode(double x, double y)
        {
            DijkstraNodeViewModel node = new(x, y);
            node.UserInteraction += NodeUserInteractionHandler;
            DijkstraObjects.Add(node);
        }

        public void CreateEdge(DijkstraNodeViewModel node1, DijkstraNodeViewModel node2)
        {
            DijkstraEdgeViewModel? edge = node1.AddEdgeIfNew(node2);
            if(edge == null) { return; }

            DijkstraObjects.Add(edge);
        }

        public void CreateEdgesFromSelected()
        {
            for (int nodeIndex = 1; nodeIndex < SelectedNodes.Count; ++nodeIndex)
            {
                CreateEdge(SelectedNodes[nodeIndex - 1], SelectedNodes[nodeIndex]);
            }
        }

        public void AddSelectedNode(DijkstraNodeViewModel node)
        {
            if (SelectedNodes.Contains(node)) { return; }
            if (SelectedNodes.Count >= MAX_SELECTED_NODES) {
                //Testing for now
                CreateEdgesFromSelected();
                SelectedNodes.Clear();
                return; 
            }

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
