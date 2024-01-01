using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class NodePositionUndoItem : IUndoItem
    {
        private BaseGraphViewModel _viewModel;
        public BaseGraphViewModel ViewModel => _viewModel;

        private Dictionary<uint, Vector2D> _initialPosition = new();
        private Dictionary<uint, Vector2D> _finalPosition = new();

        public NodePositionUndoItem(BaseGraphViewModel viewModel)
        {
            _viewModel = viewModel;
            foreach (var node in _viewModel.Nodes)
            {
                _initialPosition.Add(node.ID, new Vector2D(node.Point));
            }
        }

        public void SetFinalPosition()
        {
            foreach (var node in _viewModel.Nodes)
            {
                _finalPosition.Add(node.ID, new Vector2D(node.Point));
            }
        }

        public void Undo()
        {
            foreach (var node in _viewModel.Nodes)
            {
                if (_initialPosition.ContainsKey(node.ID))
                {
                    _viewModel.SetNodePosition(node, _initialPosition[node.ID]);
                }
            }
        }

        public void Redo()
        {
            foreach (var node in _viewModel.Nodes)
            {
                if (_finalPosition.ContainsKey(node.ID))
                {
                    _viewModel.SetNodePosition(node, _finalPosition[node.ID]);
                }
            }
        }


    }
}

