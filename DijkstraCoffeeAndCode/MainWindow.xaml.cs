// WunderVision 2023
// https://www.wundervisionengineering.com/
using DijkstraCoffeeAndCode.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace DijkstraCoffeeAndCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public GraphViewModel Graph { get; private set; }

        public double ViewWidth
        {
            get { return (double)GetValue(ViewWidthProperty); }
            set { SetValue(ViewWidthProperty, value); }
        }

        public static readonly DependencyProperty ViewWidthProperty =
            DependencyProperty.Register("ViewWidth", typeof(double), typeof(MainWindow), new PropertyMetadata(null));


        public double ViewHeight
        {
            get { return (double)GetValue(ViewHeightProperty); }
            set { SetValue(ViewHeightProperty, value); }
        }

        public static readonly DependencyProperty ViewHeightProperty =
            DependencyProperty.Register("ViewHeight", typeof(double), typeof(MainWindow), new PropertyMetadata(null));

        private const int VIEW_SIZE = 2048;
        private double _zoomLevel = 0.9;
        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = value;
                if (_zoomLevel > 1.5) { _zoomLevel = 1.5; }
                else if (_zoomLevel < 0.5) { _zoomLevel = 0.5; }

                ViewHeight = VIEW_SIZE * _zoomLevel;
                ViewWidth = VIEW_SIZE * _zoomLevel;
            }
        }




        public MainWindow()
        {
            ViewWidth = VIEW_SIZE;
            ViewHeight = VIEW_SIZE;
            ZoomLevel = 0.8;
            Graph = new GraphViewModel();
            Graph.GetFilePath = GetFilePath;
            Graph.MessageEvent += GraphMessageEvent;
            DataContext = Graph;
            InitializeComponent();
        }

        private void GraphMessageEvent(string message)
        {
            MessageBox.Show(message);
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

        private void ScaleZoom(double scale)
        {
            ZoomLevel *= scale;
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            ScaleZoom(0.8);
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            ScaleZoom(1.2);
        }
    }
}
