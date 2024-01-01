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
    public class NodeDeleteUndoItem : IUndoItem
    {
        private BaseGraphViewModel _viewModel;
        public BaseGraphViewModel ViewModel => _viewModel;

        private Node _node;

        public NodeDeleteUndoItem(BaseGraphViewModel viewModel, NodeViewModel nodeViewModel)
        {
            _viewModel = viewModel;
            _node = nodeViewModel.Node;
        }

        public void Undo()
        {
            _viewModel.AddNode(_node);
        }

        public void Redo()
        {
            _viewModel.DeleteNode(_node);
        }


    }
}

