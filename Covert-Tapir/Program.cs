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
            int pointsInSet = rand.Next(10, 20);
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < pointsInSet; i++)
            {
                int _xval = rand.Next(-100, 120);
                int _yval = rand.Next(-100, 120);
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

            var bruteWatch = Stopwatch.StartNew();
            //List<Point> testBrute = testicle.bruteForce(dataSet);
            bruteWatch.Stop();

            var quickWatch = Stopwatch.StartNew();
            List<Point> testQuick = testicle.QuickHull(dataSet);
            quickWatch.Stop();
         
            System.Console.WriteLine(watch.ElapsedMilliseconds + "ms to run point generation of " + dataSet.Count() + " points");
            System.Console.WriteLine(watchJarvis.ElapsedMilliseconds + "ms to run Jarvis; " + testJarvis.Count() + " points in hull.");
            System.Console.WriteLine(grahamWatch.ElapsedMilliseconds + "ms to run Graham; " + testGraham.Count() + " points in hull.");
            //System.Console.WriteLine(bruteWatch.ElapsedMilliseconds + "ms to run Brute; " + testBrute.Count() + " points in hull.");
            System.Console.WriteLine(quickWatch.ElapsedMilliseconds + "ms to run Quick; " + testQuick.Count() + " points in hull.");

            //foreach (Point p in testJarvis.Except(testGraham).ToList())
            //{
            //    System.Console.WriteLine(p.ToString());
            //}
            //foreach (Point p in testBrute.Except(testGraham).ToList())
            //{
            //    System.Console.WriteLine(p.ToString());
            //}

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

        public List<Point> GrahamScan(List<Point> inputSet)
        {
            // Setup
            Stack<Point> hull = new Stack<Point>();
            bool tinyHull = false;

            // Test for tiny/nonexistent hull            
            if (inputSet == null || (inputSet.Count() <= 3))
            {
                tinyHull = true;
            }

            if (!tinyHull)
            {
                // Sort array by Y
                List<Point> yPoints = sortPointListByY(inputSet);

                // Sort array again, by polar order
                //List<Point> points = polarSort(yPoints, yPoints[0]);
                var sortedPoints = (yPoints.Skip(1)).OrderByDescending(p => p, new PolarAngleComparer(yPoints[0]));
                List<Point> points = sortedPoints.ToList();
                points.Insert(0, yPoints[0]);

                //points.Sort(new PolarAngleComparer(points[0]));

                // Setup
                int N = points.Count();

                // p0 is on the hull
                hull.Push(points[0]);

                // find index k1 of first point not equal to points[0]
                int k1;
                for (k1 = 1; k1 < N; k1++)
                    if (!(points[0] == (points[k1]))) break;
                if (k1 == N) return points; // all points equal

                // find index k2 of first point not collinear with points[0] and points[k1]
                int k2;
                for (k2 = k1 + 1; k2 < N; k2++)
                    if (ccw(points[0], points[k1], points[k2]) != 0) break;
                hull.Push(points[k2 - 1]);    // points[k2-1] is second extreme point

                try
                {
                    for (int i = k2; i < N; i++)
                    {
                        Point top = hull.Pop();
                        while ((ccw(hull.Peek(), top, points[i])) > 0) // This is essentially the opposite of Sedgewick's implementation
                        {
                            top = hull.Pop();
                        }
                        hull.Push(top);
                        hull.Push(points[i]);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                }
            }
            return hull.ToList();
        }

        public List<Point> QuickHull(List<Point> dataSet)
        {
            //Setup
            List<Point> pointsOnHull = new List<Point>();
            List<Point> leftSet = new List<Point>();
            List<Point> rightSet = new List<Point>();
            
            
            if (dataSet.Count() != 0) {
                int leftX = getWesternPoint(dataSet);
                int rightX = getEasternPoint(dataSet);
                Point A = dataSet[leftX];
                Point B = dataSet[rightX];
                pointsOnHull.Add(dataSet[leftX]);
                pointsOnHull.Add(dataSet[rightX]);
                dataSet.Remove(dataSet[rightX]);
                dataSet.Remove(dataSet[leftX]);
                
                // Divide the data into points left of and right of the line
                foreach (Point i in dataSet)
                {
                    if (ccw(dataSet[leftX], dataSet[rightX], i) < 0)
                    {
                        leftSet.Add(i);
                    }
                    else
                    {
                        rightSet.Add(i);
                    }
                }

                this.hullMaker(A, B, rightSet, pointsOnHull); 
                this.hullMaker(A, B, leftSet, pointsOnHull);
            }
            return pointsOnHull;
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

        public List<Point> bruteForce(List<Point> dataSet)
        {
            List<Point> pointsOnHull = new List<Point>();
            bool tinyHull = false;
            if (dataSet == null || (dataSet.Count() <= 3) )
            {
                tinyHull = true;
            }
            if (!tinyHull)
            {
                for (int i = 0; i < dataSet.Count(); i++)
                {
                    for (int j = i+1; j < dataSet.Count(); j++)
                    {
                        bool noPointsLeft = false;
                        if (dataSet[i] != dataSet[j]) 
                        {
                            for (int k = 0; k < dataSet.Count(); k++)
                            {
                                if (dataSet[i] != dataSet[k] && dataSet[j] != dataSet[k])
                                {
                                    noPointsLeft = true;
                                    //System.Console.WriteLine(ccw(dataSet[i], dataSet[j], dataSet[k]));
                                    if (ccw(dataSet[i], dataSet[j], dataSet[k]) > 0)
                                    {
                                        noPointsLeft = false;
                                        break;
                                    }                                   
                                }
                            }
                            if (noPointsLeft && !pointsOnHull.Contains(dataSet[i]))
                            {
                                pointsOnHull.Add(dataSet[i]);
                            }
                            if (noPointsLeft && !pointsOnHull.Contains(dataSet[j]))
                            {
                                pointsOnHull.Add(dataSet[j]);
                            }
                        }                        
                    }
                }
            }
            return pointsOnHull;
        }

        public int ccw(Point a, Point b, Point point0)
        {
            int orientation = (b.X - a.X) * (point0.Y - a.Y) - (point0.X - a.X) * (b.Y - a.Y);
            if (orientation > 0)
                return -1; // point0 is to the left of the line (aX,aY)(bX,bY)
            if (orientation < 0)
                return 1; // point0 is to the right of the line (aX,aY)(bX,bY)
            return 0; // point0 is colinear with the line (aX,aY)(bX,bY)
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

        private int getEasternPoint(List<Point> input)
        {

            {
                int easternIndex = 0;
                foreach (Point p in input)
                {
                    if (p.X > input[easternIndex].X)
                    {
                        easternIndex = input.IndexOf(p);
                    }
                }
                return easternIndex;
            }
        }

        public bool isGrahamConvex(List<Point> hull) {
            int N = hull.Count();
            if (N <= 2) return true;

            List<Point> points = new List<Point>();
            foreach (Point p in hull)
            {
                points.Add(p);
            }
            System.Console.WriteLine("Test:" + points.Except(hull));

            for (int i = 0; i < N; i++)
            {
                if (ccw(points[i], points[(i + 1) % N], points[(i + 2) % N]) <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void hullMaker(Point A, Point B, List<Point> input, List<Point> hull)
        {
            // This should 
            int insertPosition = hull.IndexOf(B);
            if (input.Count() == 0) return;
            if (input.Count() == 1)
            {
                Point p = input[0];
                input.Remove(p);
                hull.Insert(insertPosition,p);
                return;
            }
            int dist = Int16.MinValue;
            int furthestPoint = -1;
            for (int i = 0; i < input.Count(); i++)
            {
                Point p = input[i];
                int distance  = calculateDistance(A,B,p);
                if (distance > dist) {
                dist = distance;
                furthestPoint = i;
                }
            }
            Point P = input[furthestPoint];
            input.Remove(input[furthestPoint]);
            hull.Insert(insertPosition,P);
    
            // Determine who's to the left of AP
            List<Point> leftSetAP = new List<Point>();
            for (int i = 0; i < input.Count(); i++)
            {
                Point M = input[i];
                if (ccw(A,P,M) > 0) {
                //set.remove(M);
                leftSetAP.Add(M);
                }
            }
    
            // Determine who's to the left of PB
            List<Point> leftSetPB = new List<Point>();
            for (int i = 0; i < input.Count(); i++)
            {
                Point M = input[i];
                if (ccw(P,B,M) > 1) {
                //set.remove(M);
                leftSetPB.Add(M);
                }
            }
            hullMaker(A, P, leftSetAP, hull);
            hullMaker(P, B, leftSetPB, hull);
        }

        private int calculateDistance(Point A, Point B, Point C)
        {
            int ABx = B.X - A.X;
            int ABy = B.Y - A.Y;
            return ABx * ABx + ABy * ABy;
        }

        public List<Point> sortPointListByY(List<Point> inputList)
        {
            //Point point;
            var sortedByY = from p in inputList
                                    orderby p.Y
                                    select p;
            return sortedByY.ToList();
        }

        public List<Point> polarSort(List<Point> inputList, Point point0)
        {
            PolarAngleComparer pac = new PolarAngleComparer(point0);
            var sortedPolarly = from p in inputList
                                orderby (pac)
                                select p;
            return sortedPolarly.ToList<Point>();
           
        }
    }

    public class PolarAngleComparer : Comparer<Point>
     {
        private Point point0;
        public PolarAngleComparer(Point point0)
        {
            this.point0 = point0;
        }         

        public override int Compare(Point a, Point b)
        {             
            if (a.Y == point0.Y || b.Y == point0.Y)
            {
                if (a.X > b.X)
                {
                    return 1;
                }
                else if (a.X < b.X)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }                 
            } else {            
                double thingA = -((double)(a.X - point0.X) / (double)(a.Y - point0.Y));
                double thingB = -((double)(b.X - point0.X) / (double)(b.Y - point0.Y));
                //System.Console.WriteLine(-((a.X - point0.X) / (a.Y - point0.Y)));
                //System.Console.WriteLine("Value: " + (thingA - thingB) );
                if (thingA < thingB)
                {
                    return 1;
                }
                else if (thingA > thingB)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
     }
}
