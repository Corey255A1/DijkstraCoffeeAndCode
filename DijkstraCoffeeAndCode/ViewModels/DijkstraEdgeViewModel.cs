using DijkstraAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraEdgeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Edge _edge;

        private bool _highlighted;
        public bool Highlighted
        {
            get { return _highlighted; }
            set
            {
                _highlighted = value;
                Notify();
            }
        }

        public double X1
        {
            get => _edge.Node1.Point.X;
        }

        public double Y1
        {
            get => _edge.Node1.Point.Y;
        }

        public double X2
        {
            get => _edge.Node2.Point.X;
        }

        public double Y2
        {
            get => _edge.Node2.Point.Y;
        }

        public double CurrentShortestDistance
        {
            get => ((DijkstraNode)_edge.Node2).ShortestRouteDistance;
        }

        public DijkstraEdgeViewModel(Edge edge)
        {
            _edge = edge;
        }

    }
}
