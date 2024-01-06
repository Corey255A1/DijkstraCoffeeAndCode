// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public class UndoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private UndoStack _undoStack;
        public UndoCommand(UndoStack undoStack)
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
            return _undoStack.HasUndoItems;
        }

        public void Execute(object? parameter)
        {
            _undoStack.Undo();
        }
    }
}
