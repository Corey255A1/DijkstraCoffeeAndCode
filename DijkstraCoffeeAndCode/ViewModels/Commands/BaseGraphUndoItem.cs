using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public abstract class BaseGraphUndoItem : IUndoItem
    {
        protected Dictionary<Node, List<Node>> _neighborCache;
        private BaseGraphViewModel _viewModel;
        public BaseGraphViewModel ViewModel => _viewModel;

        protected IEnumerable<Node> Nodes { get => _neighborCache.Keys; }
        public BaseGraphUndoItem(BaseGraphViewModel viewModel, IEnumerable<Node> nodes)
        {
            _viewModel = viewModel;
            _neighborCache = new();
            foreach (var node in nodes)
            {
                _neighborCache[node] = new List<Node>(node.Neighbors);
            }
        }
        public abstract void Redo();
        public abstract void Undo();
    }
}
