using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    internal class DijkstraNodeViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

		private DijkstraAlgorithm.DijkstraNode _node;

        private bool _selected;

		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}

		private bool _highlighted;

        public bool Highlighted
		{
			get { return _highlighted; }
			set { _highlighted = value; }
		}

		public DijkstraNodeViewModel(double x, double y)
		{
			_node = new DijkstraAlgorithm.DijkstraNode(x, y);
		}
		public DijkstraNodeViewModel()
		{
			_node = new DijkstraAlgorithm.DijkstraNode(0, 0);
		}

	}
}
