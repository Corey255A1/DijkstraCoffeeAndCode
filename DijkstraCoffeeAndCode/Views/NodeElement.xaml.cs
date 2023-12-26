// WUNDERVISION 2018
using DijkstraCoffeeAndCode.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DijkstraCoffeeAndCode.Views
{
    /// <summary>
    /// Interaction logic for NodeElement.xaml
    /// </summary>
    public partial class NodeElement : UserControl
    {
        private Point _gripPoint;
        private bool _checkingForDragThreshold = false;
        private const int PIXEL_DRAG_THRESHOLD_SQ = 25; // 5 pixels squared.

        private DijkstraNodeViewModel? _viewModel;

        public NodeElement()
        {
            InitializeComponent();
            DataContextChanged += NodeElementDataContextChanged;
        }

        private void NodeElementDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = e.NewValue as DijkstraNodeViewModel;
        }

        private void NodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) { return; }

            ((UIElement)sender).CaptureMouse();
            e.Handled = true;
            _gripPoint = Mouse.GetPosition(this);
            _checkingForDragThreshold = true;
            _viewModel.BeginInteraction();
        }

        private void NodeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null) { return; }
            if (!_viewModel.IsInteracting && !_checkingForDragThreshold) { return; }

            ((UIElement)sender).ReleaseMouseCapture();
            e.Handled = true;
            _checkingForDragThreshold = false;
            if(_viewModel.WasMovedWhileInteracting)
            {
                _viewModel.EndDrag();
            }
            _viewModel.EndInteraction();
        }

        private bool VectorLengthPassedThreshold(Point point)
        {
            return (point.X * point.X + point.Y * point.Y) > PIXEL_DRAG_THRESHOLD_SQ;
        }

        private Point GetMouseGripDeltaPosition(IInputElement element)
        {
            Point currentPoint = Mouse.GetPosition(element);
            return (Point)(currentPoint - _gripPoint);
        }

        private void NodeMouseMove(object sender, MouseEventArgs e)
        {
            if (_viewModel == null) { return; }
            if (!_viewModel.IsInteracting && !_checkingForDragThreshold) { return; }

            Point deltaPoint = GetMouseGripDeltaPosition((UIElement)sender);
            if (_checkingForDragThreshold && VectorLengthPassedThreshold(deltaPoint))
            {
                _checkingForDragThreshold = false;
                _viewModel.BeginDrag();
            }
            
            if (_viewModel.WasMovedWhileInteracting)
            {
                _viewModel.Move(deltaPoint.X, deltaPoint.Y);
            }
            e.Handled = true;
        }


    }
}
