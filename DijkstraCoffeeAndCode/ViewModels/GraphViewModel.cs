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
        public ObservableCollection<DijkstraNodeViewModel> Nodes { get; private set; } = new();
        public ObservableCollection<DijkstraEdgeViewModel> Edges { get; private set; } = new();

        public ObservableCollection<DijkstraNodeViewModel> SelectedNodes { get; private set; } = new();
        public const int MAX_SELECTED_NODES = 2;

        ICommand AddNodeCommand { get; set; }
        ICommand UpdateNodeCommand { get; set; }





        public GraphViewModel() { }

        public void AddNewNode(double x, double y)
        {
            DijkstraNodeViewModel node = new(x, y);
            node.UserInteraction += NodeUserInteraction;
            Nodes.Add(node);
        }

        private void AddSelectedNode(DijkstraNodeViewModel node)
        {
            if(SelectedNodes.Contains(node)) { return; }
            if(SelectedNodes.Count >= MAX_SELECTED_NODES) { return; }

            node.IsSelected = true;
            SelectedNodes.Add(node);
        }

        private void RemoveSelectedNode(DijkstraNodeViewModel node)
        {
            node.IsSelected = false;
            SelectedNodes.Remove(node);
        }

        private void ToggleSelectedNode(DijkstraNodeViewModel node)
        {
            if (node.IsSelected) { RemoveSelectedNode(node); }
            else { AddSelectedNode(node); }
        }

        private void NodeUserInteraction(object? sender, UserInteractionEventArgs e)
        {
            if(!(sender is DijkstraNodeViewModel node)) { return; }

            if(e.State == UserInteractionState.Begin) {
                ToggleSelectedNode(node);
            }
        }
    }
}
