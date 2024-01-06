using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class NodeDeleteUndoItem : IUndoItem
    {
        private BaseGraphViewModel _viewModel;
        private IGraphState _graphState;
        private List<Node> _neighbors;
        private Node _node;
        public NodeDeleteUndoItem(BaseGraphViewModel viewModel, NodeViewModel nodeViewModel)
        {
            _viewModel = viewModel;
            _graphState = _viewModel.GetStateSnapshot();
            _node = nodeViewModel.Node;
            _neighbors = new List<Node>(_node.Neighbors);
        }

        public void Undo()
        {
            _viewModel.AddNode(_node);
            foreach (var neighbor in _neighbors)
            {
                _viewModel.CreateEdge(_node, neighbor);
            }
            _graphState.RestoreState(_viewModel);
        }

        public void Redo()
        {
            _viewModel.DeleteNode(_node);
        }


    }
}

