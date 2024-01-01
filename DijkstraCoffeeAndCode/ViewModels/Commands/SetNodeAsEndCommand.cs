using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SetNodeAsEndCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private NodeViewModel _viewModel;
        public SetNodeAsEndCommand(NodeViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsEndNode))
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool CanExecute(object? parameter)
        {
            return !_viewModel.IsEndNode;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UserCommandSetEnd();
        }
    }
}
