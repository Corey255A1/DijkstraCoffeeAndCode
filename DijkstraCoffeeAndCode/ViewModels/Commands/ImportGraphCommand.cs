using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class ImportGraphCommand: ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private GraphViewModel _viewModel;
        public ImportGraphCommand(GraphViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.GetFilePath != null;
        }

        public void Execute(object? parameter)
        {
            _viewModel.ImportGraph();
        }
    }
}
