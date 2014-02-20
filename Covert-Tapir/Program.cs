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
        {   
            List<Point> dataSet = new List<Point>();
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                double _xval = rand.NextDouble() * (100 - (-100)) + (-100);
                double _yval = rand.NextDouble() * (100 - (-100)) + (-100);
                Point randomPoint = new Point(_xval, _yval);
                dataSet.Add(randomPoint);
            }
            /*
             * Just here for testing the random data generator works.
            foreach (Point p in dataSet) {
                System.Console.WriteLine(p);
            }
             */
            
            // Primary test section
            ConvexHull testicle = new ConvexHull();
            System.Console.WriteLine( testicle.JarvisMarch(dataSet) );
            System.Console.ReadLine();
        }

        public List<Point> JarvisMarch(List<Point> dataSet)
        {
            // Setup
            List<Point> pointsOnHull = new List<Point>(); 

            // Find leftmost point
            int leftmostIndex = 0;
            foreach (Point p in dataSet)
            {
                if (p.x < dataSet[leftmostIndex].x) {
                    leftmostIndex = dataSet.IndexOf(p);
                }
            }
            System.Console.WriteLine("Index was " + leftmostIndex);
            System.Console.WriteLine("Point was" + dataSet[leftmostIndex]);

            // Perform Jarvis' march
            return pointsOnHull;
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
