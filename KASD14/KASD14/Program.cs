using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace KASD14
{
    public class Tree
    {
        public float value;
        public float lvl;
        public Tree left = null;
        public Tree right = null;
        public Tree() { }
        public Tree(int a, int l)
        {
            value = a;
            lvl = l;
        }
        public Tree(int[] a, out List<float> lvlar)
        {
            value = a[0];
            lvl = 0;
            lvlar = new List<float>();
            lvlar.Add(1);
            Tree nowt;
            int nowlvl;
            for (int i = 1; i < a.Length; i++)
            {
                nowlvl = 0;
                nowt = this;
                while (true)
                {
                    nowlvl++;
                    if (a[i] > nowt.value)
                        if (nowt.right == null)
                        {
                            if (nowlvl >= lvlar.Count)
                                lvlar.Add(1);
                            else
                                lvlar[nowlvl]++;
                            nowt.right = new Tree(a[i], nowlvl);
                            break;
                        }
                        else
                            nowt = nowt.right;
                    else
                    if (nowt.left == null)
                    {
                        if (nowlvl >= lvlar.Count)
                            lvlar.Add(1);
                        else
                            lvlar[nowlvl]++;
                        nowt.left = new Tree(a[i], nowlvl);
                        break;
                    }
                    else
                        nowt = nowt.left;
                }
            }
        }
        static bool IsFib(long T)
        {
            double root5 = Math.Sqrt(5);
            double phi = (1 + root5) / 2;
            long idx = (long)Math.Floor(Math.Log(T * root5) / Math.Log(phi) + 0.5);
            long u = (long)Math.Floor(Math.Pow(phi, idx) / root5 + 0.5);
            return (u == T);
        }
        public bool searchFib()
        {
            int n;

            if (value > 0 && value == Math.Truncate(value))
            {
                n = (int)Math.Truncate(value);
                if (IsFib(n)) return true;
            }

            if (left != null)
            {
                if (left.searchFib())
                    return true;
            }

            if (right != null)
            {
                if (right.searchFib())
                    return true;
            }
            return false;
        }
        public void change()
        {
            if (value < 0)
                value = Math.Abs(value);
            if (left != null)
                left.change();
            if (right != null)
                right.change();
        }
        public bool isBinarySearchTree()
        {
            if (left != null && left.value > value)
                return false;
            if (right != null && right.value <= value)
                return false;
            if (left != null)
                if (right != null)
                    return left.isBinarySearchTree() && right.isBinarySearchTree();
                else
                    return left.isBinarySearchTree();
            else
            if (right != null)
                return right.isBinarySearchTree();
            else
                return true;
        }
        public void LeastCounts(float[] leafCounts)
        {
            if (left == null && right == null)
                leafCounts[(int)lvl]++;
            if (left != null)
                left.LeastCounts(leafCounts);
            if (right != null)
                right.LeastCounts(leafCounts);
        }
        static public void writeBT(Tree p, StreamWriter fstream)
        {
            try
            {
                if (p == null)
                {
                    fstream.WriteLine("-");
                }
                else
                {
                    fstream.WriteLine($"{p.value} ");
                    writeBT(p.left, fstream);
                    writeBT(p.right, fstream);
                }
            }
            catch { Console.WriteLine("Ошибка записи в файл"); return; }
        }
        static public Tree readBT(Tree p, StreamReader fin, int lv, List<float> ll)
        {
            int zna;
            bool isNumber;
            string tmp;
            try { tmp = fin.ReadLine(); zna = Convert.ToInt32(tmp); }
            catch { return null; }
            if (tmp == "-")
                return null;
            else
            {
                p.value = zna; p.lvl = lv;
                ll[lv]++;
                p.left = readBT(new Tree(), fin, lv + 1, ll);
                p.right = readBT(new Tree(), fin, lv + 1, ll);
                return p;
            }
        }
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}