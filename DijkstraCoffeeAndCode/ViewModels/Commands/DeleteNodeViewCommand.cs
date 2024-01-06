// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteNodeViewCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NodeViewModel _viewModel;
        public DeleteNodeViewCommand(NodeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UserCommandDelete();
        }
    }
}
