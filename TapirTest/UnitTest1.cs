using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Covert_Tapir
{
    [TestClass]
    public class ConvexHullTests
    {
        ConvexHull testHull = new ConvexHull();

        [TestMethod]
        public void JarvisMarchTest()
        {
            int x = 0;            
            int y = testHull.JarvisMarch;
            Assert.AreEqual(x, y);
        }

        [TestMethod]
        public void GrahamScanTest()
        {
            int x = 0;
            int y = testHull.GrahamScan;
            Assert.AreEqual(x, y);
        }
    }
}
