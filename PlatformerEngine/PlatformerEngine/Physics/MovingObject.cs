using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    public class MovingObject : PhysicsBasedObject
    {
        public Vector2 Velocity;
        public float AngularVelocity;
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                angleMatrix.X = (float)Math.Cos(angle);
                angleMatrix.Y = (float)Math.Sin(angle);
            }
        }
        private float angle;
        public float Mass;
        private Vector2 angleMatrix;
        public MovingObject(Vector2[] vertices, Vector2 position, float mass = 1) : base(vertices, position)
        {
            Mass = mass;
            Velocity = new Vector2(0, 0);
            AngularVelocity = 0;
            angleMatrix = new Vector2(0, 0);
            angle = 0;
        }
        public override Vector2 GetVertex(int num)
        {
            Vector2 actual = vertices[num];
            return new Vector2(actual.X * angleMatrix.X - actual.Y * angleMatrix.Y, actual.X * angleMatrix.Y + actual.Y * angleMatrix.X);
        }
        public override void Update()
        {
            Position += Velocity;
            Angle += AngularVelocity;
        }
    }
}
