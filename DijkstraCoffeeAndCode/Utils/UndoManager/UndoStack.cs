// WunderVision 2023
// https://www.wundervisionengineering.com/
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public class UndoStack
    {
        private Stack<IUndoItem> _undoStack = new();
        private Stack<IUndoItem> _redoStack = new();

        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

        public event EventHandler? UndoItemsChanged;

        public bool HasUndoItems => _undoStack.Count > 0;
        public bool HasRedoItems => _redoStack.Count > 0;

        public UndoStack()
        {
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        public void AddItem(IUndoItem item)
        {
            _redoStack.Clear();
            _undoStack.Push(item);
            UndoItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) { return; }

            var item = _undoStack.Pop();
            item.Undo();
            _redoStack.Push(item);
            UndoItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) { return; }

            var item = _redoStack.Pop();
            item.Redo();
            _undoStack.Push(item);
            UndoItemsChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
