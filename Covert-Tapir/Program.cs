using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Windows.Forms;

namespace Covert_Tapir
{

    public class ConvexHull
    {             
        static void Main(string[] args)
        {   
            List<Point> dataSet = new List<Point>();
            Random rand = new Random();
            int pointsInSet = rand.Next(3, 10);
            for (int i = 0; i < pointsInSet; i++)
            {
                int _xval = rand.Next(-1000, 1000);
                int _yval = rand.Next(-1000, 1000);
                Point randomPoint = new Point(_xval, _yval);
                dataSet.Add(randomPoint);
            }
            
            // Primary test section
            ConvexHull testicle = new ConvexHull();
            List<Point> testJarvis = testicle.JarvisMarch(dataSet);
            System.Console.WriteLine("Behold, the wonder of the JARVIS MARCH!");
            foreach (Point p in testJarvis)
            {
                System.Console.WriteLine(p);
            }

            System.Console.WriteLine("Feast your eyes on the astounding GRAHAM SCAN!");
            List<Point> testGraham = testicle.GrahamScan(dataSet);
            foreach (Point p in testGraham)
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
            try
            {
                foreach (Point p in dataSet)
                {
                    if (p.X < dataSet[leftmostIndex].X)
                    {
                        leftmostIndex = dataSet.IndexOf(p);
                    }
                }
                // Debug info; remove
                //System.Console.WriteLine("Index was " + leftmostIndex);
                //System.Console.WriteLine("Point was" + dataSet[leftmostIndex]);
            }
            catch (NullReferenceException e)
            {
                e.GetBaseException();
            }
            
            // Perform Jarvis' march
            try
            {
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
                            ccw(pointsOnHull[i], endpoint, dataSet[j]) > 0)
                        {
                            endpoint = dataSet[j];
                        }
                    }
                    i++;
                    pointOnHull = endpoint;
                } while (!endpoint.Equals(pointsOnHull[0]));
            }
            catch (NullReferenceException e)
            {
                e.GetBaseException();
            }
            return pointsOnHull;
        }

        public List<Point> GrahamScan(List<Point> dataSet)
        {
            // Setup
            Stack<Point> hull = new Stack<Point>();
            List<Point> points = new List<Point>(dataSet); // This may be redundant, if C# passes by value
            int N = dataSet.Count;
            bool allPointsEqual = false;
            
            // Find y-most point
            int yMostIndex = 0;
            try
            {
                foreach (Point p in points)
                {
                    if (p.Y < points[yMostIndex].Y)
                    {
                        yMostIndex = points.IndexOf(p);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                e.GetBaseException();
                System.Console.Write("The list of Points is empty.");
            }

            // Swap pointsOnHull[0] with lowest y-coord
            try
            {
                Point swap;
                swap = points[0];
                points[0] = points[yMostIndex];
                points[yMostIndex] = swap;
            }
            catch (NullReferenceException e)
            {
                System.Console.WriteLine(e.StackTrace);
            }
   
            // Sort relative to y-most point
            PolarAngleComparer pac = new PolarAngleComparer(points[0]);
            points.Sort(pac);

            // p0 is on the hull
            hull.Push(points[0]);

            // find index k1 of first point not equal to points[0]
            int k1;
            for (k1 = 1; k1 < N; k1++)
                if (!(points[0] == (points[k1]))) break;
            if (k1 == N) allPointsEqual = true;        // all points equal

            // find index k2 of first point not collinear with points[0] and points[k1]
            int k2;
            for (k2 = k1 + 1; k2 < N; k2++)
                if (ccw(points[0], points[k1], points[k2]) != 0) break;
            hull.Push(points[k2 - 1]);    // points[k2-1] is second extreme point

            // Debug info
            System.Console.WriteLine("----Test Sort----");
            foreach (Point p in points)
            {
                System.Console.WriteLine(p);
            }
            System.Console.WriteLine("----Test Sort----");

            if (!allPointsEqual)
            {
                for (int i = k2; i < N; i++)
                {
                    Point top = hull.Pop();
                    while ( (ccw(hull.Peek(), top, points[i])) > 0 )
                    {
                        top = hull.Pop();
                    }
                    hull.Push(top);
                    hull.Push(points[i]);
                }
            }
            return hull.ToList();
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

        private bool isLeft(Point a, Point b, Point c)
        {
            return ((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) > 0;
        }

        private double ccw(Point a, Point b, Point point0)
        {
            return (b.X - a.X) * (point0.Y - a.Y) - (point0.X - a.X) * (b.Y - a.Y);
        }
    }      

     class PolarAngleComparer : IComparer<Point>
     {
         private Point point0;

         /// <summary>
         /// Creates an instance of PolarAngleComparer
         /// </summary>
         /// <param name="point0">the zero (top left) point</param>
         public PolarAngleComparer(Point point0)
         {
             this.point0 = point0;
         }

         /// <summary>
         /// Compares 2 point values in order to determine the one with minimal polar angle to given zero point
         /// </summary>
         /// <param name="a">first point</param>
         /// <param name="b">second point</param>
         /// <returns>a<b => value < 0; a==b => value == 0; a>b => value > 0</returns>
         public int Compare(Point a, Point b)
         {
             return  (b.X - a.X) * (point0.Y - a.Y) - (point0.X - a.X) * (b.Y - a.Y);             
         }
     }
}
