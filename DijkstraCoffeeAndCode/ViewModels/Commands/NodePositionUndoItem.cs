// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class NodePositionUndoItem : IUndoItem
    {
        private BaseGraphViewModel _viewModel;
        public BaseGraphViewModel ViewModel => _viewModel;

        private uint _nodeId;
        private Vector2D _initialPosition;
        private Vector2D _finalPosition;

        public NodePositionUndoItem(Node node, Vector2D finalPosition, BaseGraphViewModel viewModel)
        {
            _viewModel = viewModel;
            _nodeId = node.ID;
            _initialPosition = new Vector2D(node.Point);
            _finalPosition = finalPosition;
        }

        public void Undo()
        {
            var node = _viewModel.Nodes.FirstOrDefault(node => node.ID == _nodeId);
            if(node == null) { return; }
            _viewModel.SetNodePosition(node, _initialPosition);
         }

        public void Redo()
        {
            var node = _viewModel.Nodes.FirstOrDefault(node => node.ID == _nodeId);
            if (node == null) { return; }
            _viewModel.SetNodePosition(node, _finalPosition);
        }


    }
}

