using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Collections.Generic;

namespace Covert_Tapir
{
    [TestClass]
    public class ConvexHullTests
    {
        ConvexHull testHull = new ConvexHull();

        [TestMethod]
        public void JarvisMarchTest()
        {
            Assert.IsNotNull(testHull.JarvisMarch(null));
        }

        [TestMethod]
        public void GrahamScanTest()
        {
            List<Point> testList = new List<Point>();
            Assert.IsNotNull(testHull.GrahamScan(testList));
        }

        [TestMethod]
        public void QuickHullTest()
        {
            Assert.AreEqual(0, testHull.QuickHull);
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
    }
}
