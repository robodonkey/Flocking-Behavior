using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flocking
{
    public class Swarm
    {
        public List<Boid> Boids = new List<Boid>();

        public Swarm(int boundary)
        {
            for (int i = 0; i < 20; i++)
            {
                Boids.Add(new Boid((i > 17), boundary));
            }
        }

        public void MoveBoids()
        {
            foreach (Boid boid in Boids)
            {
                boid.Move(Boids);
            }
        }
    }
}
