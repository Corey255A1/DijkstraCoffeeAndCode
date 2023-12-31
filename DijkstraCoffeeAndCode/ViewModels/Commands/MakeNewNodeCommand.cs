using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class MakeNewNodeCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;

        public MakeNewNodeCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(!(parameter is Vector2D point)) { return; }

            //_viewModel.UndoStack.AddItem(new UndoItem(_viewModel, new(_viewModel.SelectedNodes.Select(nodeView => nodeView.Node))));
            _viewModel.MakeNewNode(point.X, point.Y);
        }
    }
}
