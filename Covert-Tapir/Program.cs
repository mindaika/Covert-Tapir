using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;

namespace Covert_Tapir
{

    public class ConvexHull
    {             
        static void Main(string[] args)
        {   
            List<Point> dataSet = new List<Point>();
            Random rand = new Random();
            int pointsInSet = rand.Next(50000, 1000000);
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < pointsInSet; i++)
            {
                int _xval = rand.Next(-1000, 1000);
                int _yval = rand.Next(-1000, 1000);
                Point randomPoint = new Point(_xval, _yval);
                dataSet.Add(randomPoint);
            }
            watch.Stop();
            
            // Primary test section
            ConvexHull testicle = new ConvexHull();
            var watchJarvis = Stopwatch.StartNew();
            List<Point> testJarvis = testicle.JarvisMarch(dataSet);
            watchJarvis.Stop();

                       
            var grahamWatch = Stopwatch.StartNew();
            List<Point> testGraham = testicle.GrahamScan(dataSet);
            grahamWatch.Stop();

            System.Console.WriteLine(watch.ElapsedMilliseconds + "ms to run point generation of " + pointsInSet + " points");
            System.Console.WriteLine(watchJarvis.ElapsedMilliseconds + "ms to run Jarvis");
            System.Console.WriteLine(grahamWatch.ElapsedMilliseconds + "ms to run Graham");
            System.Console.ReadLine();            
        }

        public List<Point> JarvisMarch(List<Point> dataSet)
        {
            // Setup
            List<Point> pointsOnHull = new List<Point>();
            bool tinyHull = false;
            if (dataSet == null || (dataSet.Count() <= 3) )
            {
                tinyHull = true;
            }
            if (!tinyHull)
            {
                // Find leftmost point
                int leftmostIndex = getWesternPoint(dataSet);

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
                            ccw(pointsOnHull[i], endpoint, dataSet[j]) > 0)
                        {
                            endpoint = dataSet[j];
                        }
                    }
                    i++;
                    pointOnHull = endpoint;
                } while (!endpoint.Equals(pointsOnHull[0]));            
            }
            return pointsOnHull;
        }

        public List<Point> GrahamScan(List<Point> dataSet)
        {
            // Setup

            // Test for tiny/nonexistent hull
            Stack<Point> hull = new Stack<Point>();
            bool tinyHull = false;
            if (dataSet == null || (dataSet.Count() <= 3) )
            {
                tinyHull = true;
            }
            if (!tinyHull)
            {
                int N = dataSet.Count;
                bool allPointsEqual = false;                
                List<Point> points = new List<Point>(dataSet); // This may be redundant, if C# passes by value
                int yMostIndex = getSouthernPoint(points);

                // Swap pointsOnHull[0] with lowest y-coord            
                Point swap;
                swap = points[0];
                points[0] = points[yMostIndex];
                points[yMostIndex] = swap;

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

                if (!allPointsEqual)
                {
                    for (int i = k2; i < N; i++)
                    {
                        Point top = hull.Pop();
                        while ((ccw(hull.Peek(), top, points[i])) > 0)
                        {
                            top = hull.Pop();
                        }
                        hull.Push(top);
                        hull.Push(points[i]);
                    }
                }                
            }
            else
            {
                hull = new Stack<Point>(dataSet);                
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

        private double ccw(Point a, Point b, Point point0)
        {
            return (b.X - a.X) * (point0.Y - a.Y) - (point0.X - a.X) * (b.Y - a.Y);
        }

        private int getSouthernPoint(List<Point> input)
        {
            int southernIndex = 0;
            foreach (Point p in input)
            {
                if (p.Y < input[southernIndex].Y)
                {
                    southernIndex = input.IndexOf(p);
                }
            }
            return southernIndex;
        }

        private int getWesternPoint(List<Point> input)
        {
            int westernIndex = 0;
            foreach (Point p in input)
            {
                if (p.X < input[westernIndex].X)
                {
                    westernIndex = input.IndexOf(p);
                }
            }
            return westernIndex;
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
             // x>0 = left; x<0 = right, x=0=colinear
             return  (b.X - a.X) * (point0.Y - a.Y) - (point0.X - a.X) * (b.Y - a.Y);             
         }
     }
}
