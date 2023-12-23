using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteNodeViewCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private DijkstraNodeViewModel _viewModel;
        public DeleteNodeViewCommand(DijkstraNodeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.UserCommandDelete();
        }
    }
}
