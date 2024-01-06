// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteSelectedNodesCommand : BaseGraphCommand
    {
        public override event EventHandler? CanExecuteChanged;

        public DeleteSelectedNodesCommand(BaseGraphViewModel viewModel, UndoStack undoStack) :
            base(viewModel, undoStack)
        {
            ViewModel.SelectedNodesChanged += SelectedNodesCollectionChanged;
        }

        private void SelectedNodesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override bool CanExecute(object? parameter)
        {
            return ViewModel.SelectedNodesCount > 0;
        }

        public override void Execute(object? parameter)
        {
            UndoStack.AddItem(new UndoItem(ViewModel, ViewModel.SelectedNodes));
            ViewModel.DeleteSelectedNodes();
        }

        private class UndoItem : BaseGraphUndoItem
        {
            private IGraphState _graphState;
            public UndoItem(BaseGraphViewModel _viewModel, IEnumerable<Node> nodes) :
                base(_viewModel, nodes)
            {
                _graphState = _viewModel.GetStateSnapshot();
            }

            public override void Undo()
            {
                ViewModel.ClearSelectedNodes();
                foreach (var node in Nodes)
                {
                    ViewModel.AddNode(node);
                }

                foreach (var node in Nodes)
                {
                    foreach (var neighbor in _neighborCache[node])
                    {
                        ViewModel.CreateEdge(node, neighbor);
                    }
                }

                ViewModel.SelectNodes(Nodes);
                _graphState.RestoreState(ViewModel);
            }

            public override void Redo()
            {
                ViewModel.ClearSelectedNodes();
                ViewModel.SelectNodes(Nodes);
                ViewModel.DeleteSelectedNodes();
            }
        }
    }
}
