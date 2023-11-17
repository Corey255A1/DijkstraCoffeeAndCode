using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

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
