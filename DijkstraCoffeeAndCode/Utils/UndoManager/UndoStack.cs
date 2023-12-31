﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public class UndoStack
    {
        private Stack<IUndoItem> _undoStack = new();
        private Stack<IUndoItem> _redoStack = new();
        public UndoStack()
        {
        }

        public void AddItem(IUndoItem item)
        {
            _redoStack.Clear();
            _undoStack.Push(item);
        }

        public void Undo()
        {
            if(_undoStack.Count == 0) { return; }

            var item = _undoStack.Pop();
            item.Undo();
            _redoStack.Push(item);
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) { return; }

            var item = _redoStack.Pop();
            item.Redo();
            _undoStack.Push(item);
        }

    }
}