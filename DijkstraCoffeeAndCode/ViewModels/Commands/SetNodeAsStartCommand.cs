using DijkstraCoffeeAndCode.ViewModels;
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SetNodeAsStartCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NodeViewModel _viewModel;
        public SetNodeAsStartCommand(NodeViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsStartNode))
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool CanExecute(object? parameter)
        {
            return !_viewModel.IsStartNode;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UserCommandSetStart();
        }
    }
}
