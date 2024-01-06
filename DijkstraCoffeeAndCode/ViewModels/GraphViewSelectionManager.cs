// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class GraphViewSelectionManager
    {
        private BaseGraphViewModel _viewModel;

        protected ObservableCollection<NodeViewModel> _selectedNodes { get; private set; } = new();
        public IEnumerable<NodeViewModel> SelectedNodeViews => _selectedNodes;
        public IEnumerable<Node> SelectedNodes => _selectedNodes.Select(viewModel => viewModel.Node);

        public event NotifyCollectionChangedEventHandler SelectedNodesChanged
        {
            add { _selectedNodes.CollectionChanged += value; }
            remove { _selectedNodes.CollectionChanged -= value; }
        }
        public int SelectedNodesCount
        {
            get => _selectedNodes.Count;
        }

        public NodeViewModel? SelectedNode
        {
            get
            {
                return _selectedNodes.Count == 0 ? null : _selectedNodes[0];
            }
        }

        public GraphViewSelectionManager(BaseGraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Clear()
        {
            foreach (var node in _selectedNodes)
            {
                node.IsSelected = false;
            }
            _selectedNodes.Clear();
        }

        public void Select(NodeViewModel node, bool isMultiSelect = false)
        {
            if (_selectedNodes.Contains(node) && isMultiSelect) { return; }
            if (!isMultiSelect) { Clear(); }

            node.IsSelected = true;
            _selectedNodes.Add(node);
        }

        public void Unselect(NodeViewModel node)
        {
            node.IsSelected = false;
            _selectedNodes.Remove(node);
        }
    }
}
