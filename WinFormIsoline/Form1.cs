using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Math;

namespace WinFormIsoline
{
    public partial class Form1 : Form
    {
        const int n = 30;
        Vector3[] points = new Vector3[n];

        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = 700;
            this.Width = 670;
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);

            this.pictureBox1.Width = 620;
            this.pictureBox1.Height = 620;
           
            pictureBox1.BackColor = Color.White;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            points = new Vector3[n];
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            Pen myPen = new Pen(Color.Black, 1);

            g.TranslateTransform(310, 310);
            g.DrawLine(myPen, 0, 310, 0, -310);
            g.DrawLine(myPen, 310, 0, -310, 0);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            Random rnd = new Random();

            for (int i = 0; i < n; i++)
            {
                points[i] = new Vector3(rnd.Next(-300, 300), rnd.Next(-300, 300));
                points[i].Z = i == 0 ? 1000 : (int)(1000 - LengthPoint(points[0], points[i]));
                g.FillEllipse(redBrush, points[i].X - 4, points[i].Y - 4, 8, 8);
                g.DrawString(points[i].Z.ToString(), new Font("Helvetica", 9),
                Brushes.Black, points[i].X, points[i].Y);
            }
            DrawOutLine(g);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void DrawOutLine(Graphics g)
        {
            List<Vector3> pointOutLine = new List<Vector3>();
            int yMax = 0;
            for(int i = 1; i < n; i++)
            {

                if (points[yMax].Y > points[i].Y)
                {
                    yMax = i;
                }
            }

            Vector3 tempVec = new Vector3(10, 10);
            pointOutLine.Add(points[yMax]);
            do
            {
                int cosMin = yMax == 0 ? 1: 0;
                
                for (int i = 0; i < n; i++)
                {
                    double cosT = CalcAngl(tempVec, new Vector3(points[yMax].X - points[cosMin].X, points[yMax].Y - points[cosMin].Y));
                    double cosI = CalcAngl(tempVec, new Vector3(points[yMax].X - points[i].X, points[yMax].Y - points[i].Y));

                    if (yMax!=i && cosT < cosI)
                    {
                        
                        cosMin = i;
                    }
                }
                pointOutLine.Add(points[cosMin]);
                tempVec.X = points[yMax].X - points[cosMin].X;
                tempVec.Y = points[yMax].Y - points[cosMin].Y;
                yMax = cosMin;
            } while (pointOutLine[0] != points[yMax]);
            
            Pen myPen = new Pen(Color.Black, 1);

            List<Point> outLineDraw = new List<Point>();
            pointOutLine.ForEach(delegate (Vector3 p)
            {
                outLineDraw.Add(new Point ((int)p.X, (int)p.Y ));
            });
            g.DrawLines(myPen, outLineDraw.ToArray());
            outLineDraw.Clear();

            ConstructionIsolines(TriangleDelone(pointOutLine, g), g);                  
        }
        private List<Triangle> TriangleDelone(List<Vector3> pointOutLine, Graphics g)
        {
            Pen myPen = new Pen(Color.Purple, 1);
            List<Triangle> triangles = new List<Triangle>();
            Queue<Vector3> pointQueue = new Queue<Vector3>();
            pointOutLine.ForEach(delegate (Vector3 p)
            {
                pointQueue.Enqueue(p);
            });
            do
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        
                        if (i != j && pointQueue.Peek() != points[i] && pointQueue.Peek() != points[j] && (!pointOutLine.Contains(points[i]) || !pointOutLine.Contains(points[j])) && !ContainsTriangl(triangles, new Triangle(pointQueue.Peek(), points[i], points[j])))
                        {
                            for (int k = 0; k < n; k++)
                            {
                                if (k != i && k != j && pointQueue.Peek() != points[k] && !ConditionDelone(points[k], pointQueue.Peek(), points[i], points[j]))
                                {
                                    break;
                                }
                                if (k == n - 1)
                                {
                                    triangles.Add(new Triangle(pointQueue.Peek(), points[i], points[j]));
                                    pointQueue.Enqueue(points[i]);
                                    pointQueue.Enqueue(points[j]);
                                    //g.DrawPolygon(myPen, new Point[] { new Point((int)pointQueue.Peek().X, (int)pointQueue.Peek().Y), new Point((int)points[i].X, (int)points[i].Y), new Point((int)points[j].X, (int)points[j].Y) });
                                }
                            }
                        }

                    }
                }
                if (!pointOutLine.Contains(pointQueue.Peek()))
                {
                    pointOutLine.Add(pointQueue.Peek());
                }
                pointQueue.Dequeue();
            } while (pointQueue.Count != 0);
            return triangles;
        }

        private void ConstructionIsolines(List<Triangle> triangles, Graphics g)
        {
            Color[] color = new Color[] {
                Color.FromArgb(255, 0, 0),
                Color.FromArgb(255, 64, 0),
                Color.FromArgb(255, 128, 0),
                Color.FromArgb(255, 191, 0),
                Color.FromArgb(255, 255, 0),
                Color.FromArgb(191, 255, 0),
                Color.FromArgb(128, 255, 0),
                Color.FromArgb(64, 255, 0),
                Color.FromArgb(0, 255, 0),
                Color.FromArgb(0, 255, 64),
                Color.FromArgb(0, 255, 128)};
            Pen myPen = new Pen(color[0], (float)1.5);

            float[] h = new float[] {950, 900, 850, 800, 750, 700, 650, 600, 550, 500, 450};
            for(int i = 0; i < h.Length; i++)
            {
                myPen.Color = color[i];
                triangles.ForEach(delegate (Triangle t)
                {
                    float min = Min(Min(t.p[0].Z, t.p[1].Z), t.p[2].Z);
                    float max = Max(Max(t.p[0].Z, t.p[1].Z), t.p[2].Z);
                    if (min <= h[i] && h[i] <= max)
                    {
                        Point[] isoline = new Point[2];
                        int minIndex = t.IndexOfZ(min);
                        int maxIndex = t.IndexOfZ(max);
                        isoline[0].X = (int)PointCalc(h[i], min, max, t.p[minIndex].X, t.p[maxIndex].X);
                        isoline[0].Y = (int)PointCalc(h[i], min, max, t.p[minIndex].Y, t.p[maxIndex].Y);
                        int midIndex = t.MidIndexZ(minIndex, maxIndex);
                        if (t.p[midIndex].Z <= h[i])
                        {
                            min = t.p[midIndex].Z;
                            minIndex = midIndex;
                        }
                        else if (t.p[midIndex].Z >= h[i])
                        {
                            max = t.p[midIndex].Z;
                            maxIndex = midIndex;
                        }
                        isoline[1].X = (int)PointCalc(h[i], min, max, t.p[minIndex].X, t.p[maxIndex].X);
                        isoline[1].Y = (int)PointCalc(h[i], min, max, t.p[minIndex].Y, t.p[maxIndex].Y);


                        //g.FillEllipse(myBrush, isoline[0].X - 4, isoline[0].Y - 4, 8, 8);
                        g.DrawPolygon(myPen, isoline);
                    }
                });
            }
            

        }

        private float PointCalc(float z, float zMin, float zMax, float min, float max)
        {
            return ((max - min) * (z - zMin)) / (zMax - zMin)+min;
        }
        private bool ContainsTriangl(List<Triangle> triangles, Triangle t)
        {
            bool res = false;            
            triangles.ForEach(delegate (Triangle tr)
            {
                int numPoint = 0;
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        if(tr.p[i] == t.p[j])
                        {
                            numPoint++;
                            break;
                        }
                    }
                }             
                if (numPoint == 3)
                {
                    res = true;  
                }

            });
            return res;
        }

        private double CalcAngl(Vector3 A, Vector3 B)
        {
            return ((A.X * B.X + A.Y * B.Y) / (Sqrt(Pow(A.X, 2) + Pow(A.Y, 2)) * Sqrt(Pow(B.X, 2) + Pow(B.Y, 2))));
        }

        private double LengthPoint(Vector3 A, Vector3 B)
        {
            return Sqrt(Pow(B.X - A.X, 2) + Pow(B.Y - A.Y, 2));
        }

        private bool ConditionDelone(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            double k = Pow(p1.X, 2) + Pow(p1.Y, 2);
            double m = Pow(p2.X, 2) + Pow(p2.Y, 2);
            double n = Pow(p3.X, 2) + Pow(p3.Y, 2);
            double a = p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y);
            double b = k * (p2.Y - p3.Y) + m * (p3.Y - p1.Y) + n * (p1.Y - p2.Y);
            double c = k * (p2.X - p3.X) + m * (p3.X - p1.X) + n * (p1.X - p2.X);
            double d = k * (p2.X*p3.Y - p3.X*p2.Y) + m * (p3.X*p1.Y - p1.X*p3.Y) + n * (p1.X*p2.Y - p2.X*p1.Y);
            if(a*(Pow(p0.X, 2) + Pow(p0.Y,2)) - b*p0.X + c*p0.Y < d){
                return false;
            }
            else
            {
                return true;
            }
        }



    }
}
