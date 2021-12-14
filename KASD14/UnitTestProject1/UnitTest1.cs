using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KASD14;
using System.Collections.Generic;
using System.IO;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Tree nowTree;
            List<float> lvllist = new List<float>();
            try
            {
                int[] arra = new int[] { 1, 2, -1, 4, -2, 43, -23, 4, -2, 44, -2, 94, 2, 2, -1, -1, 23 };
                nowTree = new Tree(arra, out lvllist);
                nowTree.change();
            }
            catch { Console.WriteLine("Ошибка построения!"); return; }
            bool f1 = false;
            try
            {
                StreamWriter sw = new StreamWriter("D:\\Test.res");
                sw.WriteLine(lvllist.Count);
                Tree.writeBT(nowTree, sw);
                sw.Close();
            }
            catch { Console.WriteLine("Ошибка записи!"); return; }
            try
            {
                StreamReader ssw = new StreamReader("D:\\Test.res");
                int tmp = Convert.ToInt32(ssw.ReadLine());
                for (int i = 0; i < tmp; i++)
                    lvllist.Add(0);
                nowTree = Tree.readBT(new Tree(), ssw, 0, lvllist);
                ssw.Close();
            }
            catch { Console.WriteLine("Ошибка чтения!"); return; }
            bool f2 = nowTree.isBinarySearchTree();
            Assert.AreEqual(f1, f2);
        }
    }
}
