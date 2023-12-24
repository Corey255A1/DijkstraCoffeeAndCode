using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SetNodeAsStartCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private DijkstraNodeViewModel _viewModel;
        public SetNodeAsStartCommand(DijkstraNodeViewModel viewModel)
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
