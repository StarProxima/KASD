using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;

using ZedGraph;

namespace KASD16

{

    public partial class Form1 : Form

    {

        ZedGraphControl zedGrapgControl1 = new ZedGraphControl();
        

        public Form1()

        {

            InitializeComponent();

        }
        
        protected override void OnLoad(EventArgs e)

        {

            zedGrapgControl1.Location = new Point(10, 10);

            zedGrapgControl1.Name = "text";

            zedGrapgControl1.Size = new Size(700, 500);

            Controls.Add(zedGrapgControl1);

            GraphPane my_Pane = zedGrapgControl1.GraphPane;

            my_Pane.Title.Text = "Solution's result";

            my_Pane.XAxis.Title.Text = "My X";

            my_Pane.YAxis.Title.Text = "My Y";
            // Установим масштаб по умолчанию для оси X
            my_Pane.XAxis.Scale.Min = 1;
            my_Pane.XAxis.Scale.Max = 3;

            // Установим масштаб по умолчанию для оси Y
            my_Pane.YAxis.Scale.Min = 1;
            my_Pane.YAxis.Scale.Max = 3;

        }

        private void BTN_Clear(object sender, EventArgs e)

        {

            Clear(zedGrapgControl1);

        }

        private void Clear(ZedGraphControl zedGrapgControl1)

        {

            //GraphPane pane = Zed_GraphControl.GraphPane;

            zedGrapgControl1.GraphPane.CurveList.Clear();

            zedGrapgControl1.GraphPane.GraphObjList.Clear();

            zedGrapgControl1.GraphPane.XAxis.Type = AxisType.Linear;

            zedGrapgControl1.GraphPane.XAxis.Scale.TextLabels = null;

            zedGrapgControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;

            zedGrapgControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;

            zedGrapgControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;

            zedGrapgControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;

            zedGrapgControl1.RestoreScale(zedGrapgControl1.GraphPane);

            zedGrapgControl1.AxisChange();

            zedGrapgControl1.Invalidate();

        }

        static double f1(double x, double a)

        {

            return a/x + Math.Sqrt(x*x-1);

        }

        private void Build(ZedGraphControl Zed_GraphControl)
        {
            try
            {
                GraphPane my_Pane = Zed_GraphControl.GraphPane;

                PointPairList list = new PointPairList();
                PointPairList listPerMin = new PointPairList();
                PointPairList listPerMax = new PointPairList();

                List<double> X = new List<double>();

                List<double> Y = new List<double>();

                double h = 0.01, x = 1, a;

                a = Convert.ToDouble(textBox1.Text);

                double minY = 10, maxY = 0;
                double minX = 0, maxX = 0;

                while (x <= 3)
                {
                    list.Add(x, f1(x, a));
                    X.Add(x); Y.Add(f1(x, a));
                    x += h;
                }


                for (int i = 0; i < Y.Count; i++)
                {
                    if (Y[i] < minY)
                    {
                        minX = X[i];
                        minY = Y[i];
                    }

                    if (Y[i] > maxY)
                    {
                        maxX = X[i];
                        maxY = Y[i];
                    }
                }





                listPerMin.Add(0, minY);
                listPerMin.Add(minX, minY);
                listPerMin.Add(minX, 0);
                listPerMax.Add(0, maxY);
                listPerMax.Add(maxX, maxY);
                listPerMax.Add(maxX, 0);

                my_Pane.AddCurve(a + "/x + sqrt(x*x-1)", list, Color.Red, SymbolType.None);

                my_Pane.AddCurve("perMin " + minY, listPerMin, Color.Green, SymbolType.Circle);

                my_Pane.AddCurve("perMax " + maxY, listPerMax, Color.Blue, SymbolType.Circle);

                zedGrapgControl1.AxisChange();

                zedGrapgControl1.Invalidate();
            }
            catch(Exception e)
            {
                textBox1.Text = e +"";
            }
            

            

        }

        private void button1_Click(object sender, EventArgs e)
        { 

            Build(zedGrapgControl1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}