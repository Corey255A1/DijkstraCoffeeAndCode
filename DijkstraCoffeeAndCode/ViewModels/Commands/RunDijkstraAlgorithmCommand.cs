using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(e.PropertyName == nameof(_viewModel.StartNode) || e.PropertyName == nameof(_viewModel.EndNode))
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
            return _viewModel.StartNode != null && _viewModel.EndNode != null;
        }

        public void Execute(object? parameter)
        {
            _viewModel.RunDijkstraAlgorithm();
        }
    }
}
