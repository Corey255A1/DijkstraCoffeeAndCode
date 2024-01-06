// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class ImportGraphCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphFileManager _viewModel;
        public ImportGraphCommand(GraphFileManager viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.GetFilePath != null;
        }

        public void Execute(object? parameter)
        {
            _viewModel.ImportGraph();
        }
    }
}
