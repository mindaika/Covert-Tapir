using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covert_Tapir
{
    class TestConvexHull : ConvexHull
    {


        static void Main(string[] args)
        {
            System.Console.WriteLine("Test");
            TestConvexHull testicle = new TestConvexHull();
            System.Console.WriteLine( testicle.JarvisMarch );
            System.Console.ReadLine();
        }
    }

    abstract class ConvexHull
    {
        protected int JarvisMarch
        {
            get { return 0; }
        }

        protected int GrahamScan
        {
            get { return 0; }
        }

        protected int QuickHull
        {
            get { return 0; }
        }

        protected int DivideAndConquer
        {
            get { return 0; }
        }

        protected int MonotoneChain
        {
            get { return 0; }
        }

        protected int Incremental
        {
            get { return 0; }
        }

        protected int ICPH
        {
            get { return 0; }
        }

        protected int ChansAlgorithm
        {
            get { return 0; }
        }

    }
}
