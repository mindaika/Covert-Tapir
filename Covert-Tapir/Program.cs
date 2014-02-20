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

        private bool isLeft(Point a, Point b, Point c)
        {
            return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
        }
      
        static void Main(string[] args)
        {   
            List<Point> dataSet = new List<Point>();
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                int _xval = rand.Next(-1000, 1000);
                int _yval = rand.Next(-1000, 1000);
                //double _xval = rand.NextDouble() * (100 - (-100)) + (-100);
                //double _yval = rand.NextDouble() * (100 - (-100)) + (-100);
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
            List<Point> test = testicle.JarvisMarch(dataSet);
            foreach (Point p in test)
            {
                System.Console.WriteLine(p);
            }
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
            int i = 0;
            Point endpoint;
            Point pointOnHull = dataSet[leftmostIndex];            
            do
            {
                pointsOnHull.Add(pointOnHull);
                endpoint = dataSet[0];
                for (int j = 1; j < dataSet.Count; j++)
                {
                    if (endpoint.Equals(pointOnHull) || 
                        isLeft(pointsOnHull[i], endpoint, dataSet[j]))
                    {
                        endpoint = dataSet[j];
                    }
                }
                i++;
                pointOnHull = endpoint;
            } while (!endpoint.Equals(pointsOnHull[0]));

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
