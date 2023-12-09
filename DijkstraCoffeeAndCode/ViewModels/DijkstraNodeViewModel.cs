using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public enum UserInteractionState { Begin, Continue, End };
    public class UserInteractionEventArgs : EventArgs
    {
        public UserInteractionState State { get; set; }
    }
    public class DijkstraNodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event EventHandler<UserInteractionEventArgs>? UserInteraction;

        private DijkstraAlgorithm.DijkstraNode _node;
        public DijkstraAlgorithm.DijkstraNode Node => _node;

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; Notify(); }
        }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { _isHighlighted = value; Notify(); }
        }

        private bool _isInteracting;
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

        public DijkstraNodeViewModel(double x, double y)
        {
            _node = new DijkstraAlgorithm.DijkstraNode(x, y);
        }

        public DijkstraNodeViewModel()
        {
            _node = new DijkstraAlgorithm.DijkstraNode(0, 0);
        }

        public DijkstraEdgeViewModel? AddEdgeIfNew(DijkstraNodeViewModel node)
        {
            if(Node.FindSharedEdge(node.Node) != null)
            {
                return null;
            }
            return new DijkstraEdgeViewModel(Node.AddEdge(node.Node));
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
            RaiseUserInteraction(UserInteractionState.Begin);
        }

        public void EndInteraction()
        {
            IsInteracting = false;
            RaiseUserInteraction(UserInteractionState.End);
        }
    }
}
