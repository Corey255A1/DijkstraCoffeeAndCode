// WUNDERVISION 2018
using System.Windows;
using System.Windows.Media;

namespace DijkstraCoffeeAndCode
{
    public delegate void EdgeElementUpdatedEvent(EdgeElement obj);
    public class EdgeElement : FrameworkElement
    {
        Pen thePen = new Pen()
        {
            Brush = Brushes.Blue,
            Thickness = 5
        };
        Edge theEdge;
        public event EdgeElementUpdatedEvent EdgeDeleted;
        static Typeface font = new Typeface(new FontFamily("Calibri"), FontStyles.Normal, FontWeights.Heavy, FontStretches.Normal);

        private bool highlighted = false;
        public bool Highlighted
        {
            get
            {
                return highlighted;
            }

            set
            {
                highlighted = value;
                thePen.Brush = highlighted ? Brushes.YellowGreen :
                               Brushes.Blue;
            }
        }

        public EdgeElement(NodeElement n1, NodeElement n2)
        {
            theEdge = new Edge(n1.theNode, n2.theNode);
            theEdge.EdgeUpdated += EdgeUpdatedCB;
            theEdge.EdgeDeleted += EdgeDeletedCB;
            this.IsHitTestVisible = false;
        }

        public static bool IsAnEdge(NodeElement n1, NodeElement n2)
        {
            return Edge.GetSharedEdge(n1.theNode, n2.theNode) != null;
        }

        private void EdgeUpdatedCB(Edge e)
        {
            this.Highlighted = e.Highlighted;
            this.InvalidateVisual();
        }
        private void EdgeDeletedCB(Edge e)
        {
            EdgeDeleted?.Invoke(this);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var shadow = new FormattedText(((int)theEdge.Distance).ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                font, 16, Brushes.Black);
            var text = new FormattedText(((int)theEdge.Distance).ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                font, 16, Brushes.White);

            drawingContext.DrawLine(thePen, theEdge.N1, theEdge.N2);
            drawingContext.DrawText(shadow, theEdge.GetMidPoint(1, 1));
            drawingContext.DrawText(text, theEdge.GetMidPoint());
            base.OnRender(drawingContext);
        }

    }
}
