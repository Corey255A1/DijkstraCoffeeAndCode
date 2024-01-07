// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraAlgorithm;
using DijkstraCoffeeAndCode.ViewModels;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode
{
    public partial class MainWindow : Window
    {        
        public MainViewModel _viewModel { get; set; }

        public MainWindow()
        {
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }
        private bool AddNodeKeyDown()
        {
            return Keyboard.GetKeyStates(Key.LeftCtrl).HasFlag(KeyStates.Down) ||
                Keyboard.GetKeyStates(Key.RightCtrl).HasFlag(KeyStates.Down);
        }
        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!AddNodeKeyDown()) { return; }
            Point clickPoint = e.GetPosition(sender as FrameworkElement);
            if (_viewModel.Graph.MakeNewNodeCommand.CanExecute(null))
            {
                _viewModel.Graph.MakeNewNodeCommand.Execute(new Vector2D(clickPoint.X, clickPoint.Y));
            }
        }        

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ScaleZoom(0.8);
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ScaleZoom(1.2);
        }

        private void DayNightModeClick(object sender, RoutedEventArgs e)
        {
            _viewModel.MoveToNextColorScheme();
        }
    }
}
