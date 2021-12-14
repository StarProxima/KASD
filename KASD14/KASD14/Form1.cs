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
namespace KASD14
{
    public partial class Form1 : Form
    {
        static Tree nowTree;
        static List<float> lvllist;
        public Form1()
        {
            InitializeComponent();
        }
        private void drawTree(Graphics graphics1, List<float> l, Tree t, List<float> nodeCount, float lastNodeDeltaX, float lastNodeDeltaY, bool direction)
        {
            float delta = 150 / l.Count;
            float deltax = t.lvl / l.Count * pictureBox1.Size.Width;
            float deltay = nodeCount[(int)t.lvl] / (l[(int)t.lvl] + 1) * pictureBox1.Size.Height + pictureBox1.Size.Height / (l[(int)t.lvl] + 1f);
            RectangleF r = new RectangleF(deltax, deltay, delta, delta);
            graphics1.FillEllipse(Brushes.White, r);
            nodeCount[(int)t.lvl]++;
            if (t.lvl != 0)
            {
                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
                pen.Width = delta / 15;
                graphics1.DrawLine(pen, deltax, deltay + delta / 2, lastNodeDeltaX + delta, lastNodeDeltaY + delta / 2);
            }
            string drawString = Convert.ToString(t.value);
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", (int)(delta / 3f));
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            float x = deltax + delta / 2;
            float y = deltay + delta / 4;
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            graphics1.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            if (t.right != null)
                drawTree(graphics1, l, t.right, nodeCount, deltax, deltay, false);
            if (t.left != null)
                drawTree(graphics1, l, t.left, nodeCount, deltax, deltay, true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] arra;
            try
            {
                string[] inside = textBox1.Text.Split(' ');
                if (inside.Length == 0)
                    throw new Exception();
                arra = new int[inside.Length];
                for (int i = 0; i < inside.Length; i++)
                {
                    arra[i] = Convert.ToInt32(inside[i]);
                }
            }
            catch
            {
                textBox2.Text = "Неверный формат ввода!";
                return;
            }
            List<float> lvlar;
            Tree tree = new Tree(arra, out lvlar);
            Graphics graphics1 = Graphics.FromHwnd(pictureBox1.Handle);
            graphics1.Clear(pictureBox1.BackColor);
            List<float> nowlvlar = new List<float>();
            for (int i = 0; i < lvlar.Count; i++)
                nowlvlar.Add(0);
            drawTree(graphics1, lvlar, tree, nowlvlar, 0, 0, false);
            lvllist = lvlar;
            nowTree = tree;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nowTree == null) return;
            if (nowTree.searchFib())
            {
                nowTree.change();
                Graphics graphics1 = Graphics.FromHwnd(pictureBox1.Handle);
                graphics1.Clear(pictureBox1.BackColor);
                List<float> nowlvlar = new List<float>();
                for (int i = 0; i < lvllist.Count; i++)
                    nowlvlar.Add(0);
                drawTree(graphics1, lvllist, nowTree, nowlvlar, 0, 0, false);
                if (nowTree.isBinarySearchTree())
                    textBox2.Text = "Дерево осталось деревом двоичного поиска.";
                else
                    textBox2.Text = "Дерево перестало быть деревом двоичного поиска.";
                StreamWriter sw = new StreamWriter("D:\\Test.res");
                sw.WriteLine(lvllist.Count);
                Tree.writeBT(nowTree, sw);
                sw.Close();
            }

            else textBox2.Text = "Дерево не содержит числа Фибоначии.";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (nowTree == null) return;
            float[] treeLeafsCount = new float[lvllist.Count];
            for (int i = 0; i < lvllist.Count; i++)
                treeLeafsCount[i] = 0;
            nowTree.LeastCounts(treeLeafsCount);
            float max = treeLeafsCount[0];
            int maxi = 0;
            for (int i = 0; i < lvllist.Count; i++)
                if (treeLeafsCount[i] > max)
                {
                    max = treeLeafsCount[i];
                    maxi = i;
                }

            textBox2.Text = $"Уровень с максимальным количеством листьев: {maxi}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}