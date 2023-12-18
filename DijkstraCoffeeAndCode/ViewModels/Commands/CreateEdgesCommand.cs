using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class CreateEdgesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public CreateEdgesCommand(GraphViewModel viewModel) { 
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedNodes.Count >= 2;
        }

        public void Execute(object? parameter)
        {
            _viewModel.CreateEdgesFromSelected();
            _viewModel.ClearSelectedNodes();
        }
    }
}
