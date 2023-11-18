// WUNDERVISION 2018
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
        //public Point CanvasPosition
        //{
        //    get
        //    {
        //        return theNode.point;
        //    }
        //    set
        //    {
        //        theNode.SetPoint(value.X, value.Y);
        //        Canvas.SetLeft(this, value.X - this.Width / 2);
        //        Canvas.SetTop(this, value.Y - this.Height / 2);
        //    }
        //}
       
        public NodeElement()
        {
            //InitializeComponent();
        }

        //private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (Mouse.LeftButton == MouseButtonState.Pressed)
        //    {
        //        var offset = e.GetPosition(this);
        //        offset.X = (offset.X - (this.Width / 2)) + theNode.point.X;
        //        offset.Y = (offset.Y - (this.Height / 2)) + theNode.point.Y;
        //        CanvasPosition = offset;
        //    }
        //}
    }
}
