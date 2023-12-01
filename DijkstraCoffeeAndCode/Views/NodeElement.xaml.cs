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

        private bool _mouseDown = false;

        public NodeElement()
        {
            InitializeComponent();
            DataContextChanged += NodeElementDataContextChanged;
        }

        private void NodeElementDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = e.NewValue as DijkstraNodeViewModel;
            if (_viewModel == null) { return; }
        }

        private void NodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            e.Handled = true;
            _mouseDown = true;
            _gripPoint = Mouse.GetPosition(this);
        }

        private void NodeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseDown) { return; }
            ((UIElement)sender).ReleaseMouseCapture();
            e.Handled = true;
            _mouseDown = false;
        }

        private void NodeMouseMove(object sender, MouseEventArgs e)
        {
            if(!_mouseDown) { return; }
            Point currentPoint = Mouse.GetPosition(this);
            Point deltaPoint = (Point)(currentPoint - _gripPoint);
            _viewModel?.Move(deltaPoint.X, deltaPoint.Y);
            e.Handled = true;
        }

        
    }
}
