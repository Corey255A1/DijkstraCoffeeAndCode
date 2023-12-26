using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum UserInteractionState { BeginDrag, Continue, EndDrag, Delete, SetAsStart, SetAsEnd };
    public class UserInteractionEventArgs : EventArgs
    {
        public UserInteractionState State { get; set; }
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
            set {
                _isStartNode = value;                
                Notify();
                _isEndNode = false;
                Notify(nameof(IsEndNode));
            }
        }

        private bool _isEndNode = false;
        public bool IsEndNode
        {   
            get { return _isEndNode; }
            set { 
                _isEndNode = value;
                Notify();
                _isStartNode = false;
                Notify(nameof(IsStartNode));
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; Notify(); }
        }

        private bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { _isHighlighted = value; Notify(); }
        }

        private bool _isInteracting = false;
        public bool IsInteracting
        {
            get { return _isInteracting; }
            set { _isInteracting = value; Notify(); }
        }

        public bool WasMovedWhileInteracting { get; private set; }

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
            get => Node.Point.X - 25;
        }

        public double Top
        {
            get => Node.Point.Y - 25;
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

        public DijkstraNodeViewModel(double x, double y)
        {
            Construct(new DijkstraAlgorithm.Node(x, y));
        }

        public DijkstraNodeViewModel()
        {
            Construct(new DijkstraAlgorithm.Node(0, 0));
        }

        public DijkstraNodeViewModel(DijkstraAlgorithm.Node node)
        {
            Construct(node);
        }

        private void Construct(DijkstraAlgorithm.Node node)
        {
            _node = node;
            SetAsStartCommand = new SetNodeAsStartCommand(this);
            SetAsEndCommand = new SetNodeAsEndCommand(this);
            DeleteCommand = new DeleteNodeViewCommand(this);
        }

        public override void Reset()
        {
            RouteSegmentDistance = 0.0;
        }

        public void Move(double dX, double dY)
        {
            SetCenterPosition(X + dX, Y + dY);
        }

        public void SetCenterPosition(double x, double y)
        {
            X = x;
            Y = y;
            if (IsInteracting)
            {
                WasMovedWhileInteracting = true;
            }
        }

        private void RaiseUserInteraction(UserInteractionState state)
        {
            UserInteraction?.Invoke(this, new UserInteractionEventArgs() { State = state });
        }

        public void BeginInteraction()
        {
            IsInteracting = true;
            WasMovedWhileInteracting = false;
            RaiseUserInteraction(UserInteractionState.BeginDrag);
        }

        public void EndInteraction()
        {
            IsInteracting = false;
            RaiseUserInteraction(UserInteractionState.EndDrag);
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
