using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraEdgeViewModel
    {
        private bool _highlighted;

        public bool Highlighted
        {
            get { return _highlighted; }
            set { _highlighted = value; }
        }
    }
}
