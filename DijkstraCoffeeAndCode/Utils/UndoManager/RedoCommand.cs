// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public class RedoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private UndoStack _undoStack;
        public RedoCommand(UndoStack undoStack)
        {
            _undoStack = undoStack;
            _undoStack.UndoItemsChanged += UndoItemsChanged;
        }

        private void UndoItemsChanged(object? sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return _undoStack.HasRedoItems;
        }

        public void Execute(object? parameter)
        {
            _undoStack.Redo();
        }
    }
}
