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

        public PhysicsScene Scene
        {
            get;
            set;
        }

        public List<Shapes.CollisionShape> Shapes
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 LastPosition
        {
            get;
            set;
        }

        public Matrix4 Rotation
        {
            get;
            set;
        }

        public Matrix4 LastRotation
        {
            get;
            set;
        }

        public Vector3 Velocity
        {
            get;
            set;
        }

        public Vector3 AngularVelocity
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

        public float MomentOfInertia
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

        public bool Static
        {
            get;
            set;
        }

       
        public bool Asleep
        {
            get;
            set;
        }

        public NodeMaterial Material
        {
            get;
            set;
        }

       

        private int sleepCycles = 0;

        public PhysicsNode()
        {

            Position = Vector3.Zero;
            Force = Vector3.Zero;
            Velocity = Vector3.Zero;
            AngularVelocity = Vector3.Zero;
            Rotation = Matrix4.Identity;
            Mass = 1.0f;
            Static = false;
            Shapes = new List<Shapes.CollisionShape>();
            Asleep = false;

        }

        public PhysicsNode(Q.Scene.Nodes.SceneNode node)
        {
            Position = Vector3.Zero;
            Force = Vector3.Zero;
            Velocity = Vector3.Zero;
            AngularVelocity = Vector3.Zero;
            Rotation = Matrix4.Identity;
            Mass = 1.0f;
            Static = false;
            Shapes = new List<Shapes.CollisionShape>();
            Node = node;
            Asleep = false;
            Material = new NodeMaterial();
        }

        public void Add(Q.Harmony.Shapes.CollisionShape shape)
        {

            Shapes.Add(shape);
            shape.Node = this;

        }

        public void PhysicsToBox()
        {

            var bb = Node.GetBounds();

            var size = new Vector3();

            size.X = bb.Max.X - bb.Min.X;
            size.Y = bb.Max.Y - bb.Min.Y;
            size.Z = bb.Max.Z - bb.Min.Z;

            //  Node = node;

            // Size = size;

            float m = Mass;
            float w = size.X;
            float h = size.Y;
            float d = size.Z;
            MomentOfInertia = m * (w * w + h * h + d * d) / 12.0f;

        }

        public void ApplyTorque(float x,float y,float z)
        {

            Torque = new Vector3(x, y, z);

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

        public virtual void Simulate(float dt)
        {
            if (Asleep) return;
            LastPosition = Position;
            LastRotation = Rotation;


            Node.LocalPosition = Position;
            if (Static) return;
            Force += Mass * Scene.Gravity;
            Velocity += Force / Mass * dt;
            Position += Velocity * dt;
            Force = Vector3.Zero;
            Node.LocalPosition = Position;

            Vector3 angAccel = Torque / MomentOfInertia;

            AngularVelocity += angAccel * dt;
            Rotation *= (Matrix4.CreateRotationX(MathHelper.DegreesToRadians(AngularVelocity.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(AngularVelocity.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(AngularVelocity.Z)));

            Node.LocalRotation = Rotation;
            //Node.LocalRotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(90));


            Torque = Vector3.Zero;
     
            if(Vector3.DistanceSquared(new Vector3(0,0,0),Velocity)<0.01f)
            {
                sleepCycles++;
                if (sleepCycles > 180)
                {
                    Asleep = true;
                    Console.WriteLine("Asleep");
                }
            }
            else
            {
                sleepCycles = 0;
            }



        }

    }
}
