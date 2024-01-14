// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels.Commands;
using System;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum NodeUserInteractionState { 
        BeginInteraction, BeginDrag, ContinueDrag, 
        EndDrag, EndInteraction, Delete, SetAsStart, SetAsEnd 
    };

    public class NodeUserInteractionEventArgs : EventArgs
    {
        public NodeUserInteractionState State { get; set; }
        public object? Data { get; set; }
    }

    public class NodeViewModel : GraphObjectViewModel
    {
        public event EventHandler<NodeUserInteractionEventArgs>? UserInteraction;
        public event EventHandler<Vector2D> PositionChanged;

        private bool _settingCenterPosition = false;

        private Node _node;
        public Node Node => _node;

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

        public bool WasMovedWhileInteracting
        {
            get; private set;
        }

        public double Width
        {
            get => 50;
        }
        public double Height
        {
            get => 50;
        }

        public double X
        {
            get => Node.Point.X;
            set
            {
                if (!_settingCenterPosition)
                {
                    PositionChanged?.Invoke(this, new Vector2D(value, Y));
                }
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
                if (!_settingCenterPosition)
                {
                    PositionChanged?.Invoke(this, new Vector2D(X, value));
                }
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

        public ICommand SetAsStartCommand { get; set; }
        public ICommand SetAsEndCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public static NodeViewModel MakeNodeViewModel(Node node)
        {
            return new NodeViewModel(node);
        }


        public NodeViewModel(double x, double y)
        {
            Construct(new Node(x, y));
        }

        public NodeViewModel()
        {
            Construct(new Node(0, 0));
        }

        public NodeViewModel(Node node)
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
            base.Reset();
        }

        public void Move(double dX, double dY)
        {
            SetCenterPosition(X + dX, Y + dY);
            if (IsInteracting)
            {
                RaiseUserInteraction(NodeUserInteractionState.ContinueDrag, new Vector2D(dX, dY));
            }
        }

        public void SetCenterPosition(double x, double y)
        {
            _settingCenterPosition = true;
            X = x;
            Y = y;
            _settingCenterPosition = false;
        }

        private void RaiseUserInteraction(NodeUserInteractionState state, object? data = null)
        {
            UserInteraction?.Invoke(this, new NodeUserInteractionEventArgs() { State = state, Data = data });
        }

        public void BeginInteraction()
        {
            IsInteracting = true;
            WasMovedWhileInteracting = false;
            RaiseUserInteraction(NodeUserInteractionState.BeginInteraction);
        }

        public void BeginDrag()
        {
            WasMovedWhileInteracting = true;
            RaiseUserInteraction(NodeUserInteractionState.BeginDrag);
        }

        public void EndDrag()
        {
            RaiseUserInteraction(NodeUserInteractionState.EndDrag);
        }

        public void EndInteraction()
        {
            IsInteracting = false;
            RaiseUserInteraction(NodeUserInteractionState.EndInteraction);
        }

        public void UserCommandSetStart()
        {
            RaiseUserInteraction(NodeUserInteractionState.SetAsStart);
        }

        public void UserCommandSetEnd()
        {
            RaiseUserInteraction(NodeUserInteractionState.SetAsEnd);
        }

        public void UserCommandDelete()
        {
            RaiseUserInteraction(NodeUserInteractionState.Delete);
        }
    }
}

