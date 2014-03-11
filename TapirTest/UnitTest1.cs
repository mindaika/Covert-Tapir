using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class ConvexHullTests
    {
        ConvexHull testHull = new ConvexHull();
        List<Point> validList;
        List<Point> testSet = new List<Point>();
        

        [TestInitialize()]
        public void Setup() 
        {            
            // Create dummy data for testing
            Random rand = new Random();
            int pointsInSet = rand.Next(50, 1000);
            for (int i = 0; i < pointsInSet; i++)
            {
                int _xval = rand.Next(-1000, 1000);
                int _yval = rand.Next(-1000, 1000);
                Point randomPoint = new Point(_xval, _yval);
                testSet.Add(randomPoint);
            }
            validList = new List<Point>(testHull.GrahamScan(testSet));
        }

        [TestMethod]
        public void JarvisMarchTest()
        {
            Assert.IsNotNull(testHull.JarvisMarch(null));
            List<Point>testJarvis = testHull.JarvisMarch(testSet);
            Assert.IsFalse(testJarvis.Except(validList).Any());
        }

        [TestMethod]
        public void GrahamScanTest()
        {
            //List<Point> testList = new List<Point>();
            Assert.IsNotNull(testHull.GrahamScan(testSet));
            Assert.IsTrue(testHull.isGrahamConvex(testHull.GrahamScan(testSet)));
        }

        [TestMethod]
        public void QuickHullTest()
        {
            //Assert.AreEqual(0, testHull.QuickHull);
        }

        [TestMethod]
        public void UCPHTest()
        {
            Assert.AreEqual(0, testHull.UCPH);
        }

        [TestMethod]
        public void IncrementalTest()
        {
            Assert.AreEqual(0, testHull.Incremental);
        }

        [TestMethod]
        public void MonotoneChainTest()
        {
            Assert.AreEqual(0, testHull.MonotoneChain);
        }

        [TestMethod]
        public void ChansAlgorithmTest()
        {
            Assert.AreEqual(0, testHull.ChansAlgorithm);
        }

        [TestMethod]
        public void DivideAndConquer()
        {
            Assert.AreEqual(0, testHull.DivideAndConquer);
        }

        [TestMethod]
        public void ccwTest()
        {
            Point A = new Point(-10, -10);
            Point B = new Point(10, 10);
            Point C = new Point(-10, 10);
            Point D = new Point(10, -10);
            Point E = new Point(20, 20);
            Assert.IsTrue(testHull.ccw(A, B, C) == -1); // Confirm points to left of line
            Assert.IsTrue(testHull.ccw(A, B, D) == 1); // Confirm points to right of line
            Assert.IsTrue(testHull.ccw(A, B, E) == 0); // Confirm points colinear with line
        }

        [TestMethod]
        public void polarSortTest()
        {
            List<Point> listToSort = new List<Point>(); 
            Point A = new Point(10, -10);           
            Point B = new Point(20, -10);
            Point C = new Point(-20, -10);
            Point D = new Point(-10, -10);
            Point E = new Point(0, -10);
            Point origin = new Point(0, -10);
            PolarAngleComparer pac = new PolarAngleComparer(origin);
            listToSort.Add(origin);           
            listToSort.Add(A);
            listToSort.Add(B);
            listToSort.Add(C);
            listToSort.Add(D);
            listToSort.Add(E);
            List<Point> points = testHull.sortPointListByY(listToSort);
            //points.Sort(pac);
            //listToSort.Sort(1, listToSort.Count() - 1, pac);
            //PolarAngleComparer pac = new PolarAngleComparer(point0);
            //var sortedPolarly = from p in listToSort
            //                    orderby (p1 => p1)
            //                    select p;
            var sortedPolarly = (points.Skip(1)).OrderByDescending(p => p, pac);
            List<Point> plist = sortedPolarly.ToList();
            plist.Insert(0, points[0]);
            
            foreach (Point p in plist)
            {
                System.Console.WriteLine(p);
            }
        }
    }
}
