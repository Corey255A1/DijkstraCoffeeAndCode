using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class SetNodeAsEndCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private DijkstraNodeViewModel _viewModel;
        public SetNodeAsEndCommand(DijkstraNodeViewModel viewModel)
        {
            _viewModel = viewModel;
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
