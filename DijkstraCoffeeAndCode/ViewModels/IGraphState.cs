using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public interface IGraphState
    {
        public void StoreState(BaseGraphViewModel viewModel);
        public void RestoreState(BaseGraphViewModel viewModel);
    }
}
