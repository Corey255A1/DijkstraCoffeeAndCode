using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SaveGraphCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphFileManager _viewModel;
        public SaveGraphCommand(GraphFileManager viewModel)
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
