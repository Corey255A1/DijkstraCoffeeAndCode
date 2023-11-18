using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DijkstraCoffeeAndCode.ViewModels;
namespace DijkstraCoffeeAndCode.Controllers
{
    internal class DijkstraController
    {
        public ObservableCollection<DijkstraNodeViewModel> Nodes { get; } = new();
        public ObservableCollection<DijkstraNodeViewModel> Edges { get; } = new();

        public DijkstraController() { 
            
        }
    }
}
