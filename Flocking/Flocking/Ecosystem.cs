using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Flocking
{
    class EcoSystem : Form
    {
        private Timer timer;
        private Swarm swarm;
        private Image iconRegular;
        private Image iconZombie;

        [STAThread]
        private static void Main()
        {
            Application.Run(new EcoSystem());
        }

        public EcoSystem()
        {
            int boundary = 640;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(boundary, boundary);
            iconRegular = CreateIcon(Brushes.Blue);
            iconZombie = CreateIcon(Brushes.Red);
            swarm = new Swarm(boundary);
            timer = new Timer();
            timer.Tick += new EventHandler(this.timer_Tick);
            timer.Interval = 75;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (Boid boid in swarm.Boids)
            {
                float angle;
                if (boid.dX == 0) 
                    angle = 90f;
                else 
                    angle = (float)(Math.Atan(boid.dY / boid.dX) * 57.3);
                if (boid.dX < 0f) 
                    angle += 180f;
                Matrix matrix = new Matrix();
                matrix.RotateAt(angle, boid.Position);
                e.Graphics.Transform = matrix;
                if (boid.Zombie) 
                    e.Graphics.DrawImage(iconZombie, boid.Position);
                else 
                    e.Graphics.DrawImage(iconRegular, boid.Position);
            }
        }

        private static Image CreateIcon(Brush brush)
        {
            int x = 10;
            int y = 10;
            Bitmap icon = new Bitmap(x, y);
            Graphics g = Graphics.FromImage(icon);
            Point p1 = new Point(0, y);
            Point p2 = new Point(x, y/2);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(x/4, y/2);
            Point[] points = { p1, p2, p3, p4 };
            g.FillPolygon(brush, points);
            return icon;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            swarm.MoveBoids();
            Invalidate();
        }
    }
}
