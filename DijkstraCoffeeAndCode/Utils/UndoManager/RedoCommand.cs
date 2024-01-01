using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DijkstraCoffeeAndCode.ViewModels;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public class RedoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private UndoStack _undoStack;
        public RedoCommand(UndoStack undoStack)
        {
            _undoStack = undoStack;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _undoStack.Redo();
        }
    }
}
