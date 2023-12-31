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
            _viewModel.DeleteSelectedNodes();
        }

        private class UndoItem : IUndoItem
        {
            private List<DijkstraNodeViewModel> _selectedNodesSnapShot;
            private GraphViewModel _viewModel;
            public UndoItem(GraphViewModel viewModel, List<DijkstraNodeViewModel> snapShot)
            {
                _viewModel = viewModel;
                _selectedNodesSnapShot = snapShot;
            }

            public void Undo()
            {
                if (_selectedNodesSnapShot == null) { return; }

                _viewModel.ClearSelectedNodes();
                _viewModel.SelectNodes(_selectedNodesSnapShot);
                _viewModel.DeleteSelectedEdges();
            }

            public void Redo()
            {
                if (_selectedNodesSnapShot == null) { return; }

                _viewModel.ClearSelectedNodes();
                _viewModel.SelectNodes(_selectedNodesSnapShot);
                _viewModel.DeleteSelectedNodes();
                _viewModel.SelectNodes(_selectedNodesSnapShot);
            }
        }
    }
}
