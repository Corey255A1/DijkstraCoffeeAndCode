// WunderVision 2023
// https://www.wundervisionengineering.com

namespace DijkstraAlgorithm
{
    public delegate void VectorChangedEvent(Vector2D position);
    public class Vector2D
    {
        public event VectorChangedEvent? VectorChanged;

        private double _x;
        public double X
        {
            get { return _x; }
            set { _x = value; VectorChanged?.Invoke(this); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { _y = value; VectorChanged?.Invoke(this); }
        }

        public static double Distance(Vector2D v1, Vector2D v2)
        {
            return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow((v1.Y - v2.Y), 2));
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Distance(Vector2D v)
        {
            return Distance(this, v);
        }

    }
}
