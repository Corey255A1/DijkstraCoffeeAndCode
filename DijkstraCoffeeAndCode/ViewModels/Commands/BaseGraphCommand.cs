// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public abstract class BaseGraphCommand : ICommand
    {
        public abstract event EventHandler? CanExecuteChanged;
        private BaseGraphViewModel _viewModel;
        public BaseGraphViewModel ViewModel => _viewModel;

        private UndoStack _undoStack;
        public UndoStack UndoStack => _undoStack;

        public BaseGraphCommand(BaseGraphViewModel viewModel, UndoStack undoStack)
        {
            _viewModel = viewModel;
            _undoStack = undoStack;
        }

        public abstract bool CanExecute(object? parameter);
        public abstract void Execute(object? parameter);
    }
}
