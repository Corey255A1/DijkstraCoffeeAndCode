using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteAllEdgesCommand : BaseGraphCommand
    {
        public override event EventHandler? CanExecuteChanged;

        public DeleteAllEdgesCommand(BaseGraphViewModel viewModel, UndoStack undoStack) :
            base(viewModel, undoStack)
        { }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            UndoStack.AddItem(new UndoItem(ViewModel, ViewModel.Nodes));
            ViewModel.DeleteAllEdges();
        }


        private class UndoItem : BaseGraphUndoItem
        {
            public UndoItem(BaseGraphViewModel _viewModel, IEnumerable<Node> nodes) :
                base(_viewModel, nodes)
            {
            }

            public override void Undo()
            {
                ViewModel.ClearSelectedNodes();
                foreach (var node in Nodes)
                {
                    foreach (var neighbor in _neighborCache[node])
                    {
                        ViewModel.CreateEdge(node, neighbor);
                    }
                }

                ViewModel.SelectNodes(Nodes);
            }

            public override void Redo()
            {
                ViewModel.ClearSelectedNodes();
                ViewModel.DeleteAllEdges();
            }
        }
    }
}
