using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class GraphViewModel
    {
        public ObservableCollection<DijkstraNodeViewModel> Nodes { get; set; } = new();
        public ObservableCollection<DijkstraEdgeViewModel> Edges { get; set; } = new();

        ICommand AddNodeCommand { get; set; }
        ICommand UpdateNodeCommand { get; set; }

        public GraphViewModel() { }
    }
}
