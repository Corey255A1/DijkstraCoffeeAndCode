using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteAllNodesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public DeleteAllNodesCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UndoStack.AddItem(new UndoItem(_viewModel, _viewModel.Nodes));
            _viewModel.DeleteAllNodes();
        }


        private class UndoItem : IUndoItem
        {
            private Dictionary<Node, List<Node>> _neighborCache;
            private GraphViewModel _viewModel;
            public UndoItem(GraphViewModel viewModel, IEnumerable<Node> snapShot)
            {
                _viewModel = viewModel;
                _neighborCache = new();
                foreach (var node in snapShot)
                {
                    _neighborCache[node] = new List<Node>(node.Neighbors);
                }
            }

            public void Undo()
            {
                _viewModel.ClearSelectedNodes();
                foreach (var node in _neighborCache.Keys)
                {
                    _viewModel.AddNode(node);
                }
                foreach (var node in _neighborCache.Keys)
                {
                    foreach (var neighbor in _neighborCache[node])
                    {
                        _viewModel.CreateEdge(node, neighbor);
                    }
                }

                _viewModel.SelectNodes(_neighborCache.Keys);
            }

            public void Redo()
            {
                _viewModel.DeleteAllNodes();
            }
        }
    }
}
