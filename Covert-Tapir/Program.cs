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
                            isLeft(pointsOnHull[i], endpoint, dataSet[j]))
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
            List<Point> pointsOnHull = new List<Point>();
            pointsOnHull = dataSet;
            PolarAngleComparer pac = new PolarAngleComparer(pointsOnHull[1]);            
            pointsOnHull.Sort(pac);
            int numberOfPoints = pointsOnHull.Count;

            System.Console.WriteLine("----Test Sort----");
            foreach (Point p in pointsOnHull)
            {
                System.Console.WriteLine(p);
            }
            System.Console.WriteLine("----Test Sort----");
            // Find y-most point
            int yMostIndex = 0;
            try
            {
                foreach (Point p in dataSet)
                {
                    if (p.Y < dataSet[yMostIndex].Y)
                    {
                        yMostIndex = dataSet.IndexOf(p);
                    }
                }
                // Debug info; remove
                //System.Console.WriteLine("Index was " + yMostIndex);
                //System.Console.WriteLine("Point was" + dataSet[yMostIndex]);
            }
            catch (NullReferenceException e)
            {
                e.GetBaseException();
            }

            // Implement Graham Scan  
            // Swap pointsOnHull[1] with lowest y-coord
            Point swap;
            swap = pointsOnHull[1];
            pointsOnHull[1] = pointsOnHull[yMostIndex];
            pointsOnHull[yMostIndex] = swap;

            
            pointsOnHull[0] = pointsOnHull[numberOfPoints-1]; //Probably don't need the variable

            int M = 1;
            for (int i = 2; i < numberOfPoints; i++)
            {
                // Find next valid point on convex hull.
                while (ccw(pointsOnHull[M-1], pointsOnHull[M], pointsOnHull[i]) <= 0) {
                    if (M > 1) {
                        M -= 1;
                    }
                    else if (i==numberOfPoints) {
                        break;
                    } else {
                        i++;
                    }
                }
                // Update M and swap points[i] to the correct place.
                M++;                
                swap = pointsOnHull[M];
                pointsOnHull[M]=pointsOnHull[i];
                pointsOnHull[i]=swap;
            }
                return pointsOnHull;
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

        private int ccw(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
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
             int det = a.X * b.Y + b.X * point0.Y + point0.X * a.Y - b.Y * point0.X - point0.Y * a.X - a.Y * b.X;
             return det;
         }
     }
}
