using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class CreateEdgesCommand : BaseGraphCommand
    {
        public override event EventHandler? CanExecuteChanged;

        public CreateEdgesCommand(BaseGraphViewModel viewModel, UndoStack undoStack):
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
            return ViewModel.SelectedNodesCount > 1;
        }

        public override void Execute(object? parameter)
        {
            UndoStack.AddItem(new UndoItem(ViewModel, ViewModel.SelectedNodes));
            ViewModel.CreateEdgesFromSelected();
            ViewModel.ClearSelectedNodes();
        }

        private class UndoItem : BaseGraphUndoItem
        {
            public UndoItem(BaseGraphViewModel _viewModel, IEnumerable<Node> nodes) :
                base(_viewModel, nodes)
            { }

            public override void Undo()
            {
                ViewModel.ClearSelectedNodes();
                ViewModel.SelectNodes(Nodes);
                ViewModel.DeleteSelectedEdges();
            }

            public override void Redo()
            {
                ViewModel.ClearSelectedNodes();
                ViewModel.SelectNodes(Nodes);
                ViewModel.CreateEdgesFromSelected();
                ViewModel.SelectNodes(Nodes);
            }
        }
    }
}
