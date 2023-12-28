using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class DeleteAllEdgesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public DeleteAllEdgesCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _viewModel.DeleteAllEdges();
        }
    }
}
