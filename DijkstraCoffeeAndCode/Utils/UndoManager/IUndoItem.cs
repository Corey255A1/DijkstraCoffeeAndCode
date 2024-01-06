// WunderVision 2023
// https://www.wundervisionengineering.com/
namespace DijkstraCoffeeAndCode.Utils.UndoManager
{
    public interface IUndoItem
    {
        void Undo();
        void Redo();
    }
}
