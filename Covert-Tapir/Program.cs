using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covert_Tapir
{

    public class ConvexHull
    {
        public struct Point
        {
            public double x, y;
            // Constructor:
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            // Override the ToString method:
            public override string ToString()
            {
                return (String.Format("({0}, {1})", x, y));
            }
        }
      
        static void Main(string[] args)
        {   Point one = new Point(-16, 21.445);
            Point two = new Point();
            System.Console.WriteLine(one.ToString());
            System.Console.WriteLine(two.ToString());
            ConvexHull testicle = new ConvexHull();
            System.Console.WriteLine( testicle.UCPH );
            System.Console.ReadLine();
        }

        public List<Point> JarvisMarch(List<Point> dataSet)
        {
            return dataSet;
        }

        public int GrahamScan
        {
            get { return 0; }
        }

        public int QuickHull
        {
            get { return 0; }
        }

        public int DivideAndConquer
        {
            get { return 0; }
        }

        public int MonotoneChain
        {
            get { return 0; }
        }

        public int Incremental
        {
            get { return 0; }
        }

        public int UCPH
        {
            get { return 0; }
        }

        public int ChansAlgorithm
        {
            get { return 0; }
        }
    }
}
