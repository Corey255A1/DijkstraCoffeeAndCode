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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<NodeElement> SelectedNodes = new List<NodeElement>();
        List<NodeElement> CreatedNodes = new List<NodeElement>();
        List<EdgeElement> CreatedEdges = new List<EdgeElement>();
        DijkstraAlgo ShortestPathClass = new DijkstraAlgo();
        public MainWindow()
        {
            InitializeComponent();
            endNode.InitNode();
            Canvas.SetZIndex(endNode, 1);
            ShortestPathClass.Add(endNode.theNode);
        }

        private void graphCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType() == typeof(NodeElement) && e.Source != endNode)
            {
                var nelm = e.Source as NodeElement;
                SelectedNodes.Remove(nelm);
                ShortestPathClass.Remove(nelm.theNode);
                graphCanvas.Children.Remove(nelm);
                CreatedNodes.Remove(nelm);
                nelm.Delete();
                ShortestPathClass.ResetStepAlgo();
                stepBtn.Content = "Begin Step";
            }
        }

        private void graphCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType() != typeof(NodeElement))
            {
                Point p = e.GetPosition(graphCanvas);
                var ne = new NodeElement() { CanvasPosition = p };
                CreatedNodes.Add(ne);
                ShortestPathClass.Add(ne.theNode);
                Canvas.SetZIndex(ne, 1);
                graphCanvas.Children.Add(ne);
                ShortestPathClass.ResetStepAlgo();
                stepBtn.Content = "Begin Step";
            }
            else
            {
                var nelm = e.Source as NodeElement;
                if (!nelm.Selected && SelectedNodes.Count<2)
                {
                    SelectedNodes.Add(nelm);
                    nelm.Selected = true;
                }
                else if(nelm.Selected)
                {
                    SelectedNodes.Remove(nelm);
                    nelm.Selected = false;
                }                
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && SelectedNodes.Count==2)
            {
                if (!EdgeElement.IsAnEdge(SelectedNodes[0], SelectedNodes[1]))
                {
                    EdgeElement ee = new EdgeElement(SelectedNodes[0], SelectedNodes[1]);
                    ee.EdgeDeleted += EdgeElementDeleted;
                    graphCanvas.Children.Add(ee);
                    CreatedEdges.Add(ee);
                    SelectedNodes[0].Selected = false;
                    SelectedNodes[1].Selected = false;
                    SelectedNodes.Clear();
                }
            }
        }

        private void EdgeElementDeleted(EdgeElement e)
        {
            graphCanvas.Children.Remove(e);
            CreatedEdges.Remove(e);
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedNodes.Count>0)
            {
                if(ShortestPathClass.FindShortestPath(SelectedNodes[0].theNode, endNode.theNode))
                {
                    infoBox.Text = String.Format("Shortest Distance Found {0}", endNode.theNode.ShortestDistance);
                }
                else
                {
                    infoBox.Text = "No Path To End";
                }
            }
            else
            {
                infoBox.Text = "Please select a starting Node";
            }
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            endNode.Delete();
            endNode.theNode.Reset();
            CreatedNodes.ForEach(n => graphCanvas.Children.Remove(n));
            CreatedEdges.ForEach(n => graphCanvas.Children.Remove(n));
            SelectedNodes.Clear();
        }

        private void stepBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!ShortestPathClass.StepInProgress)
            {
                if (SelectedNodes.Count > 0)
                {
                    ShortestPathClass.BeginStepAlgo(SelectedNodes[0].theNode, endNode.theNode);
                    ShortestPathClass.TakeStep();
                }
            }
            else
            {
                ShortestPathClass.TakeStep();
            }
            if(ShortestPathClass.StepInProgress)
            {
                stepBtn.Content = "Next Step";
            }
            else
            {
                stepBtn.Content = "Begin Step";
            }
        }

        private void resetStep_Click(object sender, RoutedEventArgs e)
        {
            ShortestPathClass.ResetStepAlgo();
            stepBtn.Content = "Begin Step";
        }
    }
}
