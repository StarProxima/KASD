using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KASD15
{
    public class Node
    {
        public Point point { get; set; }
        public Color color { get; set; }

        public Node()
        {

        }

        public Node(Point point, Color color)
        {
            this.point = point;
            this.color = color;
        }

    }

    
    public class Graph
    {
        static float rad = 60;
        public int size = 0;
        public List<List<Tuple<int, int>>> adjacencyList = new List<List<Tuple<int, int>>>();
        public List<Node> nodes = new List<Node>();

        public void AddVertex(int x, int y)
        {
            List<Tuple<int, int>> newl = new List<Tuple<int, int>>();
            adjacencyList.Add(newl);
            nodes.Add(new Node(new Point(x, y), Color.White));
            size++;
        }
        public void AddEdge(int v1, int v2, int weight = int.MaxValue)
        {
            Random rand = new Random();
            if(weight == int.MaxValue)
                adjacencyList[v1].Add(new Tuple<int, int>(v2, rand.Next(-100, 100)));
            else adjacencyList[v1].Add(new Tuple<int, int>(v2, weight));
        }
        public void Remove(int v)
        {
            adjacencyList.Remove(adjacencyList[v]);
            for (int i = 0; i < adjacencyList.Count; i++)
            {
                for (int j = 0; j < adjacencyList[i].Count; j++)
                {
                    if (adjacencyList[i][j].Item1 == v)
                    {
                        adjacencyList[i].RemoveRange(j, 1);
                    }

                }
            }
            for (int i = 0; i < adjacencyList.Count; i++)
            {
                for (int j = 0; j < adjacencyList[i].Count; j++)
                {
                    if (adjacencyList[i][j].Item1 > v)
                    {
                        adjacencyList[i][j] = new Tuple<int, int>(adjacencyList[i][j].Item1 - 1, adjacencyList[i][j].Item2);
                    }
                }
            }
            nodes.RemoveRange(v, 1);
            size--;
        }

        public bool IsEdge(int v1, int v2)
        {
            for (int i = 0; i < adjacencyList[v1].Count; i++)
            {
                if (adjacencyList[v1][i].Item1 == v2) return true;
            }
            return false;
        }
        public int IsOnPoint(int x, int y)
        {
            for (int i = 0; i < size; i++)
            {
                if (Math.Sqrt((double)((x - nodes[i].point.X) * (x - nodes[i].point.X) + (y - nodes[i].point.Y) * (y - nodes[i].point.Y))) < rad / 2)
                    return i;
            }
            return -1;
        }
        public int IsNearPoint(float x, float y)
        {
            for (int i = 0; i < size; i++)
            {
                if (x == nodes[i].point.X && y == nodes[i].point.Y) continue;
                if (Math.Sqrt((double)((x - nodes[i].point.X) * (x - nodes[i].point.X) + (y - nodes[i].point.Y) * (y - nodes[i].point.Y))) <= rad)
                    return i;
            }
            return -1;
        }
        public void SaveGraph()
        {

            StreamWriter sw = new StreamWriter("D:\\G.grf");
            sw.WriteLine(adjacencyList.Count);
            for (int i = 0; i < adjacencyList.Count; i++)
            {
                string s = $"{nodes[i].point.X} {nodes[i].point.Y}:";
                for (int j = 0; j < adjacencyList[i].Count; j++)
                {
                    s += +adjacencyList[i][j].Item1 + "," + adjacencyList[i][j].Item2 + " ";

                }
                sw.WriteLine(s);
            }

            sw.Close();

        }
        public void LoadGraph()
        {
            adjacencyList = new List<List<Tuple<int, int>>>();
            nodes = new List<Node>();

            StreamReader sw = new StreamReader("D:\\G.grf");
            int size1 = Convert.ToInt32(sw.ReadLine());

            for (int i = 0; i < size1; i++)
            {
                string s = sw.ReadLine(), s1 = "", s2 = "";
                int j = 0;

                for (; j < s.Length; j++)
                {
                    if (s[j] + "" == ":") break;
                    else if (s[j] + "" == " ")
                    {
                        s2 = s1;
                        s1 = "";

                    }
                    else s1 += s[j] + "";
                }


                nodes.Add(new Node(new Point(Convert.ToInt32(s2), Convert.ToInt32(s1)), Color.White));
                s1 = ""; s2 = "";

                List<Tuple<int, int>> newl = new List<Tuple<int, int>>();
                for (j = j + 1; j < s.Length; j++)
                {

                    if (s[j] + "" == " ")
                    {

                        newl.Add(new Tuple<int, int>(Convert.ToInt32(s2), Convert.ToInt32(s1)));
                        s1 = ""; s2 = "";
                    }
                    else if (s[j] + "" == ",")
                    {
                        s2 = s1;
                        s1 = "";

                    }
                    else s1 += s[j] + "";
                }

                adjacencyList.Add(newl);

            }

            size = size1;
            sw.Close();

        }

        public List<int> NegativeCycle()
        {
            const int INF = (int)1e9;
            int n = adjacencyList.Count;
            List<int> path = new List<int>();

            int x = -1, sizeE = 0;
            List<int> d = new List<int>(), p = new List<int>();
            for (int i = 0; i < n; i++)
            {
                nodes[i].color = Color.White;
                d.Add(INF);
                p.Add(-1);
                sizeE += adjacencyList[i].Count;
            }

            d[0] = 0;

            for (int i = 0; i < n; i++)
            {
                x = -1;
                for (int j = 0; j < adjacencyList.Count; j++)
                {
                    for (int k = 0; k < adjacencyList[j].Count; k++)
                    {
                        int from = j;
                        int to = adjacencyList[j][k].Item1;
                        int cost = adjacencyList[j][k].Item2;
                        if (d[to] > d[from] + cost)
                        {
                            d[to] = Math.Max(d[from] + cost, -INF);
                            p[to] = from;
                            x = to;
                        }
                    }

                }
            }
            if (x == -1)
            {
                return path;
            }
            else
            {
                int y = x;
                for (int i = 0; i < n; i++)
                {
                    y = p[y];
                }

                for (int cur = y; ; cur = p[cur])
                {
                    path.Add(cur);
                    nodes[cur].color = Color.Red;
                    if (cur == y && path.Count > 1)
                    {
                        break;
                    }
                }


                return path;
            }

        }
    }

    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
