using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteSelectedNodesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public DeleteSelectedNodesCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.SelectedNodes.CollectionChanged += SelectedNodesCollectionChanged;
        }

        private void SelectedNodesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedNodes.Count > 0;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UndoStack.AddItem(new UndoItem(_viewModel, _viewModel.SelectedNodes));
            _viewModel.DeleteSelectedNodes();
        }

        private class UndoItem : IUndoItem
        {
            private Dictionary<Node, List<Node>> _neighborCache;
            private GraphViewModel _viewModel;
            public UndoItem(GraphViewModel viewModel, IEnumerable<DijkstraNodeViewModel> snapShot)
            {
                _viewModel = viewModel;
                _neighborCache = new();
                foreach(var node in snapShot)
                {
                    _neighborCache[node.Node] = new List<Node>(node.Node.Neighbors);
                }
            }

            public void Undo()
            {
                _viewModel.ClearSelectedNodes();
                foreach(var node in _neighborCache.Keys)
                {
                    _viewModel.AddNode(node);
                }

                foreach (var node in _neighborCache.Keys)
                {
                    foreach(var neighbor in _neighborCache[node])
                    {
                        _viewModel.CreateEdge(node, neighbor);
                    }                    
                }

                _viewModel.SelectNodes(_neighborCache.Keys);
            }

            public void Redo()
            {
                _viewModel.ClearSelectedNodes();
                _viewModel.SelectNodes(_neighborCache.Keys);
                _viewModel.DeleteSelectedNodes();
            }
        }
    }
}
