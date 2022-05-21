using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Harmony
{

    public class Collision
    {
        public Vector3 HitPoint
        {
            get;
            set;
        }

        public PhysicsNode Node1
        {
            get;
            set;
        }

        public PhysicsNode Node2
        {
            get;
            set;
        }

        public float IntersectionLength
        {
            get;
            set;
        }

        public float CollisionForce
        {
            get;
            set;
        }

        public float HitTime
        {
            get;
            set;
        }

        public Vector3 HitNormal
        {
            get;
            set;
        }

    }

    public class PhysicsNode
    {

        public Vector3 Position
        {
            get;
            set;
        }

        public Matrix4 Rotation
        {
            get;
            set;
        }


        public Vector3 Velocity
        {
            get;
            set;
        }

        public Vector3 Force
        {
            get;
            set;
        }

        public float Mass
        {
            get;
            set;
        }

        public Vector3 Torque
        {
            get;
            set;
        }

        public Vector3 InertialDrag
        {
            get;
            set;
        }

        public Vector3 AngularDrag
        {
            get;
            set;
        }

        public PhysicsNode()
        {

            Position = Vector3.Zero;
            Force = Vector3.Zero;
            Velocity = Vector3.Zero;
            Mass = 1.0f;


        }

        public Q.Scene.Nodes.SceneNode Node
        {
            get;
            set;
        }

        public virtual Collision CollideWith(PhysicsNode node2)
        {
            return null;
        }

    }
}
