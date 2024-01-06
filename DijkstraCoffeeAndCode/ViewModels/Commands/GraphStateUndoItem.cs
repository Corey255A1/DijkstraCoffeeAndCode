using DijkstraCoffeeAndCode.Utils.UndoManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels.Commands
{
    public class GraphStateUndoItem : IUndoItem
    {
        IGraphState _previousGraphState;
        IGraphState _currentGraphState;
        BaseGraphViewModel _viewModel;
        public GraphStateUndoItem(BaseGraphViewModel viewModel, IGraphState previousGraphState)
        {
            _viewModel = viewModel;
            _previousGraphState = previousGraphState;
            _currentGraphState = viewModel.GetStateSnapshot();
            
        }
        public void Redo()
        {
            _currentGraphState.RestoreState(_viewModel);
        }

        public void Undo()
        {
            _previousGraphState.RestoreState(_viewModel);
        }
    }
}
