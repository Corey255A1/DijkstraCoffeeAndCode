// WUNDERVISION 2018
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace DijkstraCoffeeAndCode
{
    public delegate void EdgeUpdatedEvent(Edge obj);
    public class Edge
    {
        public Node N1;
        public Node N2;
        public Point Mid;

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
                EdgeUpdated?.Invoke(this);
            }

        }

        public EdgeUpdatedEvent EdgeUpdated;
        public EdgeUpdatedEvent EdgeDeleted;
        public Edge(Node n1, Node n2)
        {
            N1 = n1;
            N1.Edges.Add(this);
            N1.NodeUpdated += NodeUpdatedCB;
           
            N2 = n2;
            N2.Edges.Add(this);
            N2.NodeUpdated += NodeUpdatedCB;
        }

        private void NodeUpdatedCB(Node obj, NodeProperties prop)
        {
            if (prop == NodeProperties.POINT)
            {
                EdgeUpdated?.Invoke(this);
            }
        }
        public void Delete()
        {
            EdgeDeleted?.Invoke(this);
        }

        public static Edge GetSharedEdge(Node n1, Node n2)
        {
            return n1.Edges.Find(e => e.GetEnd(n1) == n2);
        }

        public double Distance
        {
            get { return Math.Sqrt(Math.Pow(N1.point.X - N2.point.X, 2) + Math.Pow((N1.point.Y - N2.point.Y), 2)); }
        }

        public Point GetMidPoint(int xoffset=0, int yoffset=0)
        {
            Mid.X = (N1.point.X + N2.point.X) / 2 + xoffset;
            Mid.Y = (N1.point.Y + N2.point.Y) / 2 + yoffset;
            return Mid;
        }

        public Node GetEnd(Node nThis)
        {
            return N1 == nThis ? N2 : N1;
        }

    }
}
