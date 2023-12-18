﻿using DijkstraAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
    public class DijkstraNodeViewModel : DijkstraObjectViewModel
    {
        public event EventHandler<UserInteractionEventArgs>? UserInteraction;

        private DijkstraAlgorithm.DijkstraNode _node;
        public DijkstraAlgorithm.DijkstraNode Node => _node;

        public event EdgeEvent? EdgeEvent { 
            add => _node.EdgeEvent += value; 
            remove => _node.EdgeEvent -= value;
        }

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

        public double ZIndex
        {
            get => 1.0;
        }

        public DijkstraNodeViewModel(double x, double y)
        {
            Construct(new DijkstraAlgorithm.DijkstraNode(x, y));
        }

        public DijkstraNodeViewModel()
        {
            Construct(new DijkstraAlgorithm.DijkstraNode(0, 0));
        }

        private void Construct(DijkstraAlgorithm.DijkstraNode node)
        {
            _node = node;
        }

        public void AddEdge(DijkstraNodeViewModel node)
        {
            Node.AddEdge(node.Node);
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

        public void RemoveAllEdges()
        {
            _node.RemoveAllEdges();
        }
    }
}
