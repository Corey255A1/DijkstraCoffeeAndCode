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
            _viewModel.BeginInteraction();

        }

        private void NodeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel == null || !_viewModel.IsInteracting) { return; }

            ((UIElement)sender).ReleaseMouseCapture();
            e.Handled = true;
            _viewModel.EndInteraction();
        }

        private Point GetMouseGripDeltaPosition(IInputElement element)
        {
            Point currentPoint = Mouse.GetPosition(element);
            return (Point)(currentPoint - _gripPoint);
        }

        private void NodeMouseMove(object sender, MouseEventArgs e)
        {
            if (_viewModel == null || !_viewModel.IsInteracting) { return; }

            Point deltaPoint = GetMouseGripDeltaPosition((UIElement)sender);
            _viewModel.Move(deltaPoint.X, deltaPoint.Y);
            e.Handled = true;
        }


    }
}
