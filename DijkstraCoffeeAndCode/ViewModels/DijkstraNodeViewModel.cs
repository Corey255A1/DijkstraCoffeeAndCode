using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraNodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private DijkstraAlgorithm.DijkstraNode _node;

        public DijkstraAlgorithm.DijkstraNode Node => _node;

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

        public double X
        {
            get => Node.Point.X;
            set { Node.Point.X = value; }
        }

        public double Y
        {
            get => Node.Point.Y;
            set { Node.Point.Y = value; }
        }

        public double Left
        {
            get => Node.Point.X - 25;
        }

        public double Top
        {
            get => Node.Point.Y - 25;
        }

        public DijkstraNodeViewModel(double x, double y)
        {
            _node = new DijkstraAlgorithm.DijkstraNode(x, y);
        }

        public DijkstraNodeViewModel()
        {
            _node = new DijkstraAlgorithm.DijkstraNode(0, 0);
        }

        public void Move(double dX, double dY)
        {
            SetCenterPosition(X + dX, Y + dY);
        }

        public void SetCenterPosition(double x, double y)
        {
            X = x;
            Y = y;
            Notify(nameof(Left));
            Notify(nameof(Top));
        }
    }
}
