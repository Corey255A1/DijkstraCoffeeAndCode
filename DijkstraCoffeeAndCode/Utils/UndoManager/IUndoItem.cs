using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public interface IUndoItem
    {
        void Undo();
        void Redo();
    }
}
