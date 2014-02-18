using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Covert_Tapir;

namespace TapirTest
{
    [TestClass]
    public class ConvexHullTests
    {
        [TestMethod]
        public void JarvisMarchTest()
        {
            int x = 0;
            ConvexHull testHull = new ConvexHull();
            int y = testHull.JarvisMarch;
            Assert.AreEqual( x, y );
        }
    }
}
