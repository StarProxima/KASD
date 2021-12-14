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
    
    public partial class Form1 : Form
    {
        static float rad = 60;




        Graph g = new Graph();
        int pointTaken = -1;
        
        int lineFromPoint = -1;
        
        static float foundSin(Point x, Point y)
        {
            return Math.Abs(x.Y - y.Y) / ((float)Math.Sqrt((x.X - y.X) * (x.X - y.X) + (x.Y - y.Y) * (x.Y - y.Y)));
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try 
            {
                g.LoadGraph();
                textBox1.Text = "Загружено.";
            }
            catch { textBox1.Text = "Ошибка загрузки!"; }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try 
            {
                g.SaveGraph();
                textBox1.Text = "Сохранено.";
            }
            catch { textBox1.Text = "Ошибка сохранения!"; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            List<int> path = g.NegativeCycle();
            if (path.Count != 0)
            {
                string s = "Отрицательный цикл: ";
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    s += path[i] + " ";
                }
                textBox1.Text = s;
            }
            else textBox1.Text = "Отрицательный цикл не найден.";
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pointTaken = -1;
            if (e.Button == MouseButtons.Right)
            {
                int tmp = g.IsOnPoint(e.Location.X, e.Y);
                if (tmp != -1)
                {
                    lineFromPoint = tmp;
                }
                else if (g.IsNearPoint(e.X, e.Y) == -1)
                {
                    g.AddVertex(e.X, e.Y);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                int tmp = g.IsOnPoint(e.Location.X, e.Y);
                if (tmp != -1)
                {
                    pointTaken = tmp;
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (lineFromPoint != -1 && e.Button == MouseButtons.Right)
            {
                int tmp = g.IsOnPoint(e.Location.X, e.Y);
                if (tmp == lineFromPoint)
                {
                    g.Remove(tmp);
                }
                else if (tmp != -1)
                {
                    if (!g.IsEdge(lineFromPoint, tmp))
                    {
                        g.AddEdge(lineFromPoint, tmp);
                        g.AddEdge(tmp, lineFromPoint);
                    }

                }

            }
            else if (pointTaken != -1 && e.Button == MouseButtons.Left)
            {
                g.nodes[pointTaken].point = new Point(pictureBox1.PointToClient(MousePosition).X, pictureBox1.PointToClient(MousePosition).Y);
            }
            pointTaken = -1;
            lineFromPoint = -1;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pointTaken != -1 && e.Button == MouseButtons.Left)
            {
                g.nodes[pointTaken].point = new Point(pictureBox1.PointToClient(MousePosition).X, pictureBox1.PointToClient(MousePosition).Y);
            }
            
        }
       
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap buffer = new Bitmap(Width, Height);
            Graphics gfx = Graphics.FromImage(buffer);

            
            
            

            gfx.Clear(pictureBox1.BackColor);
            

            Pen penOutline = new Pen(Color.Gray);
            Pen penEdge = new Pen(Color.Gray);
            penOutline.Width = 4;
            penEdge.Width = 4;

            SolidBrush brushNode = new SolidBrush(Color.Black);

            for (int i = 0; i < g.size; i++)
            {
                for (int j = i+1; j < g.size; j++)
                {
                    if (g.IsEdge(i,j))
                    {
                        float angS = foundSin(g.nodes[i].point, g.nodes[j].point);
                        float angC = (float)Math.Sqrt(1 - angS * angS);
                        if (g.nodes[i].point.Y < g.nodes[j].point.Y)
                            angS = -angS;
                        if (g.nodes[i].point.X < g.nodes[j].point.X)
                            angC = -angC;

                        Point point1 = new Point(g.nodes[i].point.X, g.nodes[i].point.Y);
                        Point point2 = new Point(g.nodes[j].point.X, g.nodes[j].point.Y);
                        gfx.DrawLine(penEdge, point1, new Point(g.nodes[j].point.X + (int)(rad / 2 * angC), g.nodes[j].point.Y + (int)(rad / 2 * angS)));
                        int k;
                        for (k = 0; k < g.adjacencyList[i].Count; k++)
                            if (g.adjacencyList[i][k].Item1 == j) break;
                        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                        drawFormat.Alignment = StringAlignment.Center;
                        gfx.DrawString(g.adjacencyList[i][k].Item2 + "", new Font("Arial", 14, FontStyle.Regular), brushNode, new PointF((point1.X + point2.X)/2, (point1.Y  + point2.Y)/2 -10), drawFormat);
                        
                    }
                }
            }
            if (lineFromPoint != -1)
            {
                gfx.DrawLine(penEdge,
                    new Point(g.nodes[lineFromPoint].point.X, g.nodes[lineFromPoint].point.Y),
                    new Point(pictureBox1.PointToClient(MousePosition).X, pictureBox1.PointToClient(MousePosition).Y));
            }
            for (int i = 0; i < g.size; i++)
            {
                brushNode.Color = g.nodes[i].color;
                gfx.FillEllipse(brushNode, new Rectangle(g.nodes[i].point.X - (int)(rad / 2), g.nodes[i].point.Y - (int)(rad / 2), (int)rad, (int)rad));
                brushNode.Color = Color.Black;
                gfx.DrawString(i.ToString(), new Font("Arial", 14, FontStyle.Regular), brushNode, new PointF(g.nodes[i].point.X - rad / 4, g.nodes[i].point.Y - 10));
                gfx.DrawEllipse(penOutline, new Rectangle(g.nodes[i].point.X - (int)(rad / 2), g.nodes[i].point.Y - (int)(rad / 2), (int)rad, (int)rad));
            }
            pictureBox1.Image = buffer;
            brushNode.Dispose();
            
            penOutline.Dispose();
            penEdge.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

