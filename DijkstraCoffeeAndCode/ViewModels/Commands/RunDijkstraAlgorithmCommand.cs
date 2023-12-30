// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class RunDijkstraAlgorithmCommand : ICommand
    {
        private GraphViewModel _viewModel;
        public event EventHandler? CanExecuteChanged;

        public RunDijkstraAlgorithmCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.StartNode) ||
                e.PropertyName == nameof(_viewModel.EndNode) ||
                e.PropertyName == nameof(_viewModel.SelectedExecutionMode))
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public void InvokeCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.StartNode != null &&
                _viewModel.EndNode != null &&
                _viewModel.SelectedExecutionMode == AlgorithmExecutionModeEnum.Manual;
        }

        public void Execute(object? parameter)
        {
            _viewModel.RunDijkstraAlgorithm();
        }
    }
}
