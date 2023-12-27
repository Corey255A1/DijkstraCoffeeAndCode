using DijkstraAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraCoffeeAndCode.ViewModels
{
    public class DijkstraEdgeViewModel : DijkstraObjectViewModel
    {
        private Edge _edge;
        public Edge Edge => _edge;

        public double Left => 0;
        public double Top => 0;
        public double ZIndex => 0;

        public double X1
        {
            get => _edge.Node1.Point.X;
        }

        public double Y1
        {
            get => _edge.Node1.Point.Y;
        }

        public double X2
        {
            get => _edge.Node2.Point.X;
        }

        public double Y2
        {
            get => _edge.Node2.Point.Y;
        }

        public double CenterX
        {
            get => X1 + ((X2 - X1) / 2);
        }

        public double CenterY
        {
            get => Y1 + ((Y2 - Y1) / 2);
        }

        public double Distance
        {
            get => _edge.Distance;
        }

        public static DijkstraEdgeViewModel MakeEdgeViewModel(Edge edge)
        {
            return new DijkstraEdgeViewModel(edge);
        }

        public DijkstraEdgeViewModel(Edge edge)
        {
            _edge = edge;
            _edge.Node1.Point.VectorChanged += Node1PositionChanged;
            _edge.Node2.Point.VectorChanged += Node2PositionChanged;
        }

        public override void Reset()
        {
            base.Reset();
        }

        private void NotifyPositions()
        {
            Notify(nameof(CenterX));
            Notify(nameof(CenterY));
            Notify(nameof(Distance));
        }

        private void Node1PositionChanged(Vector2D position)
        {
            Notify(nameof(X1));
            Notify(nameof(Y1));
            NotifyPositions();
        }

        private void Node2PositionChanged(Vector2D position)
        {
            Notify(nameof(X2));
            Notify(nameof(Y2));
            NotifyPositions();
        }
    }
}
