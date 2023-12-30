using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SaveGraphCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public SaveGraphCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.GetFilePath != null;
        }

        public void Execute(object? parameter)
        {
            bool isSaveAs = false;
            if (parameter != null && parameter.GetType() == typeof(bool))
            {
                isSaveAs = (bool)parameter;
            }
            _viewModel.SaveGraph(isSaveAs);
        }
    }
}
