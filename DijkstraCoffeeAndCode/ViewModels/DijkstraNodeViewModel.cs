// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum UserInteractionState { BeginInteraction, BeginDrag, ContinueDrag, EndDrag, EndInteraction, Delete, SetAsStart, SetAsEnd };
    public class UserInteractionEventArgs : EventArgs
    {
        public UserInteractionState State { get; set; }
        public object? Data { get; set; }
    }
    public class DijkstraNodeViewModel : DijkstraObjectViewModel
    {
        public event EventHandler<UserInteractionEventArgs>? UserInteraction;

        private DijkstraAlgorithm.Node _node;
        public DijkstraAlgorithm.Node Node => _node;

        private bool _isStartNode = false;
        public bool IsStartNode
        {
            get { return _isStartNode; }
            set
            {
                if (_isStartNode == value) { return; }
                _isStartNode = value;
                Notify();
                if (_isStartNode)
                {
                    IsEndNode = false;
                }
            }
        }

        private bool _isEndNode = false;
        public bool IsEndNode
        {
            get { return _isEndNode; }
            set
            {
                if (_isEndNode == value) { return; }
                _isEndNode = value;
                Notify();
                if (_isEndNode)
                {
                    IsStartNode = false;
                }
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; Notify(); }
        }

        private bool _isInteracting = false;
        public bool IsInteracting
        {
            get { return _isInteracting; }
            set { _isInteracting = value; Notify(); }
        }

        private bool _isVisited = false;
        public bool IsVisited
        {
            get { return _isVisited; }
            set { _isVisited = value; Notify(); }
        }

        public bool WasMovedWhileInteracting { get; private set; }

        public double Width { get => 50; }
        public double Height { get => 50; }

        public double X
        {
            get => Node.Point.X;
            set
            {
                Node.Point.X = value;
                Notify();
                Notify(nameof(Left));
            }
        }

        public double Y
        {
            get => Node.Point.Y;
            set
            {
                Node.Point.Y = value;
                Notify();
                Notify(nameof(Top));
            }
        }

        public double Left
        {
            get => Node.Point.X - (Width / 2);
        }

        public double Top
        {
            get => Node.Point.Y - (Height / 2);
        }

        public double ZIndex
        {
            get => 1.0;
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

        public ICommand SetAsStartCommand { get; set; }
        public ICommand SetAsEndCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public static DijkstraNodeViewModel MakeNodeViewModel(Node node)
        {
            return new DijkstraNodeViewModel(node);
        }


        public DijkstraNodeViewModel(double x, double y)
        {
            Construct(new Node(x, y));
        }

        public DijkstraNodeViewModel()
        {
            Construct(new Node(0, 0));
        }

        public DijkstraNodeViewModel(Node node)
        {
            Construct(node);
        }

        private void Construct(Node node)
        {
            _node = node;
            SetAsStartCommand = new SetNodeAsStartCommand(this);
            SetAsEndCommand = new SetNodeAsEndCommand(this);
            DeleteCommand = new DeleteNodeViewCommand(this);
        }

        public override void Reset()
        {
            RouteSegmentDistance = 0.0;
            IsVisited = false;
            base.Reset();
        }

        public void Move(double dX, double dY)
        {
            SetCenterPosition(X + dX, Y + dY);
            if (IsInteracting)
            {
                RaiseUserInteraction(UserInteractionState.ContinueDrag, new Vector2D(dX, dY));
            }
        }

        public void SetCenterPosition(double x, double y)
        {
            X = x;
            Y = y;
        }

        private void RaiseUserInteraction(UserInteractionState state, object? data = null)
        {
            UserInteraction?.Invoke(this, new UserInteractionEventArgs() { State = state, Data = data });
        }

        public void BeginInteraction()
        {
            IsInteracting = true;
            WasMovedWhileInteracting = false;
            RaiseUserInteraction(UserInteractionState.BeginInteraction);
        }

        public void BeginDrag()
        {
            WasMovedWhileInteracting = true;
            RaiseUserInteraction(UserInteractionState.BeginDrag);
        }

        public void EndDrag()
        {
            RaiseUserInteraction(UserInteractionState.EndDrag);
        }

        public void EndInteraction()
        {
            IsInteracting = false;
            RaiseUserInteraction(UserInteractionState.EndInteraction);
        }

        public void UserCommandSetStart()
        {
            RaiseUserInteraction(UserInteractionState.SetAsStart);
        }

        public void UserCommandSetEnd()
        {
            RaiseUserInteraction(UserInteractionState.SetAsEnd);
        }

        public void UserCommandDelete()
        {
            RaiseUserInteraction(UserInteractionState.Delete);
        }
    }
}
