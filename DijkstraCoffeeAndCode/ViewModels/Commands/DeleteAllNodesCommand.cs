// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteAllNodesCommand : BaseGraphCommand
    {
        public override event EventHandler? CanExecuteChanged;
        public DeleteAllNodesCommand(BaseGraphViewModel viewModel, UndoStack undoStack) :
            base(viewModel, undoStack)
        {
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            UndoStack.AddItem(new UndoItem(ViewModel, ViewModel.Nodes));
            ViewModel.DeleteAllNodes();
        }


        private class UndoItem : BaseGraphUndoItem
        {
            public UndoItem(BaseGraphViewModel _viewModel, IEnumerable<Node> nodes) :
                base(_viewModel, nodes)
            { }

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

                ViewModel.SelectNodes(_neighborCache.Keys);
            }

            public override void Redo()
            {
                ViewModel.DeleteAllNodes();
            }
        }
    }
}
