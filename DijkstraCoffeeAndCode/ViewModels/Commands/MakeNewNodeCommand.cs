// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class MakeNewNodeCommand : BaseGraphCommand
    {
        public override event EventHandler? CanExecuteChanged;

        public MakeNewNodeCommand(BaseGraphViewModel viewModel, UndoStack undoStack) :
            base(viewModel, undoStack)
        {
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            if (!(parameter is Vector2D point)) { return; }

            Node newNode = ViewModel.MakeNewNode(point.X, point.Y);
            UndoStack.AddItem(new UndoItem(ViewModel, new List<Node> { newNode }));
        }

        private class UndoItem : BaseGraphUndoItem
        {
            public UndoItem(BaseGraphViewModel _viewModel, IEnumerable<Node> nodes) :
                base(_viewModel, nodes)
            {
            }
            public override void Undo()
            {
                ViewModel.DeleteNode(Nodes.First());
            }

            public override void Redo()
            {
                ViewModel.AddNode(Nodes.First());
            }
        }
    }
}
