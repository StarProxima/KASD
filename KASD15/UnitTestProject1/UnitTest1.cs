using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KASD15;
using System.Collections.Generic;
namespace KASD15
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Graph g;
            g = new Graph();
            g.AddVertex(0, 0);
            g.AddVertex(100, 0);
            g.AddVertex(100, 100);
            g.AddVertex(0, 100);
            g.AddEdge(0, 1, 22);
            g.AddEdge(0, 2, -15);
            g.AddEdge(0, 3, 23);
            g.AddEdge(1, 3, -13);
            g.AddEdge(3, 1, -13);
            g.AddEdge(1, 2, 13);
            g.AddEdge(2, 3, -74);
            g.SaveGraph();
            g.adjacencyList.RemoveRange(0, 4);
            g.nodes.RemoveRange(0, 4);
            g.LoadGraph();
            List<int> path = g.NegativeCycle();
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(3, path[0]);
            Assert.AreEqual(2, path[1]);
            Assert.AreEqual(1, path[2]);
        }
    }
}
