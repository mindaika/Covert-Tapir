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
            // Primary test section
            ConvexHull testicle = new ConvexHull();

            for (int i = 0; i < 10; i++)
            {
                testicle.testProcedure();
                System.Console.WriteLine("");
            }
            System.Console.ReadLine();            
        }

        public List<Point> JarvisMarch(List<Point> inputSet)
        {
            // Setup
            List<Point> pointsOnHull = new List<Point>();
            if (inputSet == null || (inputSet.Count() <= 3) )
            {
                return inputSet;
            }

            //
            int k1;
            for (k1 = 1; k1 < inputSet.Count(); k1++)
                if (!(inputSet[0] == (inputSet[k1]))) break;
            if (k1 == inputSet.Count()) return inputSet; // all points equal

            // Find leftmost point
            int leftmostIndex = getWesternPoint(inputSet);

            // Perform Jarvis' march                
            int i = 0;
            Point endpoint;
            Point pointOnHull = inputSet[leftmostIndex];
            do
            {
                pointsOnHull.Add(pointOnHull);
                endpoint = inputSet[0];
                for (int j = 1; j < inputSet.Count; j++)
                {
                    if (endpoint.Equals(pointOnHull) ||
                        // This ensures the next move is a turn, but fails to capture collinear points
                        ccw(pointsOnHull[i], endpoint, inputSet[j]) > 0)
                    {
                        endpoint = inputSet[j];
                    }
                }
                i++;
                pointOnHull = endpoint;
            } while (!endpoint.Equals(pointsOnHull[0]));            
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
                return inputSet;
            }

            // Sort array by Y
            List<Point> yPoints = sortPointListByY(inputSet);

            // Sort array again, by polar order
            var sortedPoints = (yPoints.Skip(1)).OrderByDescending(p => p, new PolarAngleComparer(yPoints[0])).ThenBy(p => p.X);
            List<Point> points = sortedPoints.ToList();
            points.Insert(0, yPoints[0]);

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

            
            for (int i = k2; i < N; i++)
            {
                Point top = hull.Pop();
                while (ccw(hull.Peek(), top, points[i]) <= 0) // This is essentially the opposite of Sedgewick's implementation
                {
                    top = hull.Pop();
                }
                hull.Push(top);
                hull.Push(points[i]);
            }

            if (isGrahamConvex(hull.Reverse().ToList()))
            {
                System.Console.WriteLine("Hull is confirmed as convex.");
            }
            return hull.Reverse().ToList();
        }

        public List<Point> QuickHull(List<Point> inputSet)
        {
            //Setup
            List<Point> pointsOnHull = new List<Point>();
            List<Point> leftSet = new List<Point>();
            List<Point> rightSet = new List<Point>();
            
            
            if (inputSet.Count() != 0) {
                int leftX = getWesternPoint(inputSet);
                int rightX = getEasternPoint(inputSet);
                Point A = inputSet[leftX];
                Point B = inputSet[rightX];
                pointsOnHull.Add(inputSet[leftX]);
                pointsOnHull.Add(inputSet[rightX]);
                //inputSet.Remove(inputSet[rightX]);
                //inputSet.Remove(inputSet[leftX]);
                
                // Divide the data into points left of and right of the line
                foreach (Point i in inputSet)
                {
                    if (ccw(inputSet[leftX], inputSet[rightX], i) < 0)
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

        public List<Point> bruteForce(List<Point> inputSet)
        {
            List<Point> pointsOnHull = new List<Point>();
            bool tinyHull = false;
            if (inputSet == null || (inputSet.Count() <= 3) )
            {
                tinyHull = true;
            }
            if (!tinyHull)
            {
                for (int i = 0; i < inputSet.Count(); i++)
                {
                    for (int j = 0; j < inputSet.Count(); j++)
                    {
                        bool noPointsLeft = false;
                        if (inputSet[i] != inputSet[j]) 
                        {
                            for (int k = 0; k < inputSet.Count(); k++)
                            {
                                if (inputSet[i] != inputSet[k] && inputSet[j] != inputSet[k])
                                {
                                    noPointsLeft = true;
                                    if (ccw(inputSet[i], inputSet[j], inputSet[k]) > 0)
                                    {
                                        noPointsLeft = false;
                                        break;
                                    }                                   
                                }
                            }
                            if (noPointsLeft && !pointsOnHull.Contains(inputSet[i]))
                            {
                                pointsOnHull.Add(inputSet[i]);
                            }
                            if (noPointsLeft && !pointsOnHull.Contains(inputSet[j]))
                            {
                                pointsOnHull.Add(inputSet[j]);
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
                return 1; // point0 is to the left of the line (aX,aY)(bX,bY)
            else if (orientation < 0)
                return -1; // point0 is to the right of the line (aX,aY)(bX,bY)
            else return 0; // point0 is colinear with the line (aX,aY)(bX,bY)
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
            var sorted = inputList.OrderBy(p => p.Y).ThenBy(p => p.X);       
            return sorted.ToList();
        }

        public List<Point> polarSort(List<Point> inputList, Point point0)
        {
            PolarAngleComparer pac = new PolarAngleComparer(point0);
            var sortedPolarly = from p in inputList
                                orderby (pac)
                                select p;
            return sortedPolarly.ToList<Point>();           
        }

        private void testProcedure()
        {
            List<Point> rawData = new List<Point>();
            Random rand = new Random();
            int pointsInSet = rand.Next(1, 10000);
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < pointsInSet; i++)
            {
                int _xval = rand.Next(-10000, 10000);
                int _yval = rand.Next(-10000, 10000);
                Point randomPoint = new Point(_xval, _yval);
                if (!rawData.Contains(randomPoint))
                    rawData.Add(randomPoint);
            }
            watch.Stop();
            // There. Now you CAN'T ruin it.
            List<Point> dataSet = new List<Point>(rawData);            

            var watchJarvis = Stopwatch.StartNew();
            List<Point> testJarvis = this.JarvisMarch(dataSet);
            watchJarvis.Stop();

            var grahamWatch = Stopwatch.StartNew();
            List<Point> testGraham = this.GrahamScan(dataSet);
            grahamWatch.Stop();

            var bruteWatch = Stopwatch.StartNew();
            List<Point> testBrute = this.bruteForce(dataSet);
            bruteWatch.Stop();

            var quickWatch = Stopwatch.StartNew();
            List<Point> testQuick = this.QuickHull(dataSet);
            quickWatch.Stop();

            System.Console.WriteLine(watch.ElapsedMilliseconds + "ms to run point generation of " + rawData.Count() + " points");
            System.Console.WriteLine(watchJarvis.ElapsedMilliseconds + "ms to run Jarvis; " + testJarvis.Count() + " points in hull.");
            System.Console.WriteLine(grahamWatch.ElapsedMilliseconds + "ms to run Graham; " + testGraham.Count() + " points in hull.");
            System.Console.WriteLine(bruteWatch.ElapsedMilliseconds + "ms to run Brute; " + testBrute.Count() + " points in hull.");
            System.Console.WriteLine(quickWatch.ElapsedMilliseconds + "ms to run Quick; " + testQuick.Count() + " points in hull.");

            foreach (Point p in testJarvis.Except(testGraham).ToList())
            {
                System.Console.WriteLine(p.ToString());
            }

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
