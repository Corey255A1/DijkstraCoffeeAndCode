// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraNodeViewModel : NodeViewModel
    {
        private bool _isVisited = false;
        public bool IsVisited
        {
            get { return _isVisited; }
            set { _isVisited = value; Notify(); }
        }

        private double _routeSegmentDistance = 0.0;
        public double RouteSegmentDistance
        {
            get => _routeSegmentDistance;
            set
            {
                _routeSegmentDistance = value;
                Notify();
            }
        }

        public static new NodeViewModel MakeNodeViewModel(Node node)
        {
            return new DijkstraNodeViewModel(node);
        }


        public DijkstraNodeViewModel(double x, double y) : base(x, y)
        {
        }

        public DijkstraNodeViewModel() : base()
        {
        }

        public DijkstraNodeViewModel(Node node) : base(node)
        {
        }

        public override void Reset()
        {
            RouteSegmentDistance = 0.0;
            IsVisited = false;
            base.Reset();
        }
    }
}
