﻿using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteSelectedEdgesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public DeleteSelectedEdgesCommand(GraphViewModel viewModel)
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
            return _viewModel.SelectedNodes.Count > 1;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UndoStack.AddItem(new UndoItem(_viewModel, new(_viewModel.SelectedNodes)));
            _viewModel.DeleteSelectedEdges();
            _viewModel.ClearSelectedNodes();
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
                _viewModel.CreateEdgesFromSelected();
                _viewModel.SelectNodes(_selectedNodesSnapShot);

            }

            public void Redo()
            {
                if (_selectedNodesSnapShot == null) { return; }

                _viewModel.ClearSelectedNodes();
                _viewModel.SelectNodes(_selectedNodesSnapShot);
                _viewModel.DeleteSelectedEdges();
            }
        }
    }
}
