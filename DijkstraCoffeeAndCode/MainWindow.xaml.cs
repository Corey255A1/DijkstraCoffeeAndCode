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
            Point clickPoint = e.GetPosition(sender as FrameworkElement);
            Graph.AddNewNode(clickPoint.X, clickPoint.Y);
        }
    }
}
