using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Flocking
{
    public class Boid
    {
        private static Random random = new Random();
        private static float border = 85f;
        private static float sight = 70f;
        private static float space = 20f;
        private static float speed = 10f;
        private float boundary;
        public float dX;
        public float dY;
        public bool Zombie;
        public PointF Position;

        public Boid(bool zombie, int boundary)
        {
            Position = new PointF(random.Next(boundary), random.Next(boundary));
            this.boundary = boundary;
            Zombie = zombie;
        }

        public void Move(List<Boid> boids)
        {
            if (!Zombie) 
                Flock(boids);
            else 
                Hunt(boids);
            CheckBounds();
            CheckSpeed();
            Position.X += dX;
            Position.Y += dY;
        }

        private void Flock(List<Boid> boids)
        {
            foreach (Boid boid in boids)
            {
                float distance = Distance(Position, boid.Position);
                if (boid != this && !boid.Zombie)
                {
                    if (distance < space)
                    {
                        // Create space.
                        dX += Position.X - boid.Position.X;
                        dY += Position.Y - boid.Position.Y;
                    }
                    else if (distance < sight)
                    {
                        // Flock together.
                        dX += (boid.Position.X - Position.X) * 0.05f;
                        dY += (boid.Position.Y - Position.Y) * 0.05f;
                    }
                    if (distance < sight)
                    {
                        // Align movement.
                        dX += boid.dX * 0.5f;
                        dY += boid.dY * 0.5f;
                    }
                }
                if (boid.Zombie && distance < sight)
                {
                    // Avoid zombies.
                    dX += Position.X - boid.Position.X;
                    dY += Position.Y - boid.Position.Y;
                }
            }
        }

        private void Hunt(List<Boid> boids)
        {
            float range = float.MaxValue;
            Boid prey = null;
            foreach (Boid boid in boids)
            {
                if (!boid.Zombie)
                {
                    float distance = Distance(Position, boid.Position);
                    if (distance < sight && distance < range)
                    {
                        range = distance;
                        prey = boid;
                    }
                }
            }
            if (prey != null)
            {
                // Move towards closest prey.
                dX += prey.Position.X - Position.X;
                dY += prey.Position.Y - Position.Y;
            }
        }

        private static float Distance(PointF p1, PointF p2)
        {
            double val = Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
            return (float)Math.Sqrt(val);
        }

        private void CheckBounds()
        {
            float val = boundary - border;
            if (Position.X < border) 
                dX += border - Position.X;
            if (Position.Y < border) 
                dY += border - Position.Y;
            if (Position.X > val) 
                dX += val - Position.X;
            if (Position.Y > val) 
                dY += val - Position.Y;
        }

        private void CheckSpeed()
        {
            float s;
            if (!Zombie) s = speed;
            else s = speed / 2f;
            float val = Distance(new PointF(0f, 0f), new PointF(dX, dY));
            if (val > s)
            {
                dX = dX * s / val;
                dY = dY * s / val;
            }
        }
    }
}
