using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class ResetGraphCommand : ICommand
    {
        private DijkstraGraphViewModel _viewModel;
        public event EventHandler? CanExecuteChanged;

        public ResetGraphCommand(DijkstraGraphViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedExecutionMode))
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedExecutionMode == AlgorithmExecutionModeEnum.Manual;
        }

        public void Execute(object? parameter)
        {
            _viewModel.ResetAllDijkstraViewObjects();
        }
    }
}
