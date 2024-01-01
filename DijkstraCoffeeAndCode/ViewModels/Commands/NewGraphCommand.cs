using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class NewGraphCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphFileManager _viewModel;
        public NewGraphCommand(GraphFileManager viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.NewGraph();
        }
    }
}
