// WUNDERVISION 2018
using DijkstraCoffeeAndCode.ViewModels;
using Microsoft.Win32;
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



        public int ViewWidth
        {
            get { return (int)GetValue(ViewWidthProperty); }
            set { SetValue(ViewWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewWidthProperty =
            DependencyProperty.Register("ViewWidth", typeof(int), typeof(MainWindow), new PropertyMetadata(null));



        public int ViewHeight
        {
            get { return (int)GetValue(ViewHeightProperty); }
            set { SetValue(ViewHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewHeightProperty =
            DependencyProperty.Register("ViewHeight", typeof(int), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow()
        {
            ViewWidth = 2048;
            ViewHeight = 2048;
            Graph = new GraphViewModel();
            Graph.GetFilePath = GetFilePath;
            DataContext = Graph;
            InitializeComponent();
        }

        private string GetFilePath(bool isOpen, string extensions)
        {
            if (isOpen)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = extensions;
                if (openFileDialog.ShowDialog() == true)
                {
                    return openFileDialog.FileName;
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = extensions;
                if (saveFileDialog.ShowDialog() == true)
                {
                    return saveFileDialog.FileName;
                }
            }
            return "";
        }

        private void CanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(sender as FrameworkElement);
            Graph.AddNewNode(clickPoint.X, clickPoint.Y);
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            ViewHeight /= 2;
            ViewWidth /= 2;
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            ViewHeight *= 2;
            ViewWidth *= 2;
        }
    }
}
