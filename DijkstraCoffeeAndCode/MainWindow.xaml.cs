// WUNDERVISION 2018
using DijkstraCoffeeAndCode.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public GraphViewModel Graph { get; private set; }
        public MainWindow()
        {
            Graph = new GraphViewModel();
            DataContext = Graph;
            InitializeComponent();
        }

        private void CanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DijkstraNodeViewModel node = new DijkstraNodeViewModel();
            Point clickPoint = e.GetPosition(sender as FrameworkElement);
            node.SetCenterPosition(clickPoint.X, clickPoint.Y);
            Graph.Nodes.Add(node);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void stepBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void resetStep_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
