// WUNDERVISION 2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DijkstraCoffeeAndCode
{
    /// <summary>
    /// Interaction logic for NodeElement.xaml
    /// </summary>
    public partial class NodeElement : UserControl
    {
        public Node theNode;
        private bool visited;
        public bool Visited
        {
            get
            {
                return visited;
            }
            set
            {
                visited = value;
                nodeEllipse.Fill = selected ? Brushes.Green :
                                   visited ? Brushes.Blue :                              
                                   Brushes.White;
            }
        }
        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                nodeEllipse.Fill = selected ? Brushes.Green : Brushes.White;
            }
        }
        public Brush Overlay
        {
            get
            {
                return (Brush)GetValue(OverlayProperty);
            }
            set
            {
                SetValue(OverlayProperty, value);
            }
        }
        public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register(
            "Overlay", typeof(Brush), typeof(NodeElement), new PropertyMetadata(default(Brush), OnOverlayPropertyChanged));
        private static void OnOverlayPropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            ((NodeElement)dobj).overlayRect.Fill = (Brush)dargs.NewValue;
        }
        public Point CanvasPosition
        {
            get
            {
                return theNode.point;
            }
            set
            {
                theNode.SetPoint(value.X, value.Y);
                Canvas.SetLeft(this, value.X - this.Width / 2);
                Canvas.SetTop(this, value.Y - this.Height / 2);
            }
        }
        public void InitNode()
        {
            theNode.point = new Point(Canvas.GetLeft(this) + this.Width / 2, Canvas.GetTop(this) + this.Height / 2);
        }

        private string ShortestDistanceText
        {
            get
            {
                return shortestDistanceLabel.Content.ToString();
            }
            set
            {
                if (value == "")
                {
                    shortestDistanceLabel.Visibility = Visibility.Hidden;
                }
                else
                {
                    shortestDistanceLabel.Content = value;
                    shortestDistanceLabel.Visibility = Visibility.Visible;
                }
            }
        }

        public NodeElement()
        {
            InitializeComponent();
            theNode = new Node();
            theNode.NodeUpdated += NodeUpdatedCB;
            ShortestDistanceText = "";
        }

        public void Delete()
        {
            theNode.Delete();
        }

        private void NodeUpdatedCB(Node n, NodeProperties prop)
        {
            if(prop == NodeProperties.HIGHLIGHT)
            {
                if(n.Highlight)
                {
                    nodeEllipse.Fill = Brushes.YellowGreen;
                }
                else
                {
                    nodeEllipse.Fill = selected ? Brushes.Green :
                                       visited ? Brushes.Blue:
                                       Brushes.White;
                }
            }
            else if(prop == NodeProperties.VIZUALIZED)
            {
                if (n.Visualized)
                {
                    nodeEllipse.Fill = Brushes.Red;
                }
                else
                {
                    nodeEllipse.Fill = selected ? Brushes.Green :
                                       visited ? Brushes.Blue :
                                       Brushes.White;
                }
            }
            else if(prop == NodeProperties.VISITED)
            {
                Visited = n.Visited;
            }
            else if(prop == NodeProperties.SHORTESTDISTANCE)
            {
                if(n.ShortestDistance == int.MaxValue)
                {
                    ShortestDistanceText = "";
                }
                else
                {
                    ShortestDistanceText = n.ShortestDistance.ToString();
                }
            }
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var offset = e.GetPosition(this);
                offset.X = (offset.X - (this.Width / 2)) + theNode.point.X;
                offset.Y = (offset.Y - (this.Height / 2)) + theNode.point.Y;   
                CanvasPosition = offset;
            }
        }
    }
}
