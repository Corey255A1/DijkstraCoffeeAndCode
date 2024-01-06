using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DijkstraCoffeeAndCode.ViewModels;

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
