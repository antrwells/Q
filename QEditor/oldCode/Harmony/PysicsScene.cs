using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Ext;
namespace Q.Harmony
{
    public class CollisionPair
    {
        public PhysicsNode A, B;

        public CollisionPair()
        {
            A = null;
            B = null;
        }
    }

    public class CollisionEvent
    {
        public CollisionPair Pair
        {
            get;
            set;
        }
        public CollisionResult Result
        {
            get;
            set;
        }
    }

    public class PhysicsScene
    {

        public List<PhysicsNode> Nodes
        {
            get;
            set;
        }

        public Vector3 Gravity
        {
            get;
            set;
        }

        public List<CollisionEvent> Collisions
        {
            get;
            set;
        }

        public PhysicsScene()
        {

            Nodes = new List<PhysicsNode>();
            Gravity = new Vector3(0, -0.2f, 0);
            // Gravity = new Vector3(0, 0, 0);
            Collisions = new List<CollisionEvent>();
        }

        public void Add(PhysicsNode node)
        {
            node.Scene = this;
            Nodes.Add(node);
        }

        public void Add(params PhysicsNode[] nodes)
        {
            foreach(var node in nodes)
            {
                Add(node);
            }
        }

        public void Remove(PhysicsNode node)
        {
            Nodes.Remove(node);
        }
        int lastTick = 0;
        public void Update(float time_delta)
        {

            if (lastTick == 0)
            {
                lastTick = Environment.TickCount;
                return;
            }

            int tc = Environment.TickCount - lastTick;
            if (tc == 0)
            {
                tc = 1;
            }

            float td = tc / 1000.0f;

            
           // time_delta = td;

            Collisions.Clear();

            //Simulate
            foreach (var node in Nodes)
            {
                node.Simulate(time_delta);
            }

            List<CollisionPair> Pairs = new List<CollisionPair>();

            //Generate possible contacts
            foreach (var node in Nodes)
            {
                foreach (var other in Nodes)
                {
                    if (other == node)
                    {
                        continue;
                    }

                    bool found = false;
                    foreach (var pair in Pairs)
                    {
                        if (pair.A == node && pair.B == other)
                        {
                            found = true;
                            break;
                        }
                        if (pair.A == other && pair.B == node)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        continue;
                    }
                    CollisionPair npair = new CollisionPair();
                    npair.A = node;
                    npair.B = other;
                    Pairs.Add(npair);



                }
            }


            //Perform possible contacts collision list.
            foreach (var pair in Pairs)
            {
                var node = pair.A;
                var other = pair.B;


                node.Shapes[0].Position = node.Position;
                other.Shapes[0].Position = other.Position;

                var res = CollisionTest.Intersect(node.Shapes[0], other.Shapes[0]);
                if (res.Collided)
                {
                    CollisionEvent ce = new CollisionEvent();
                    ce.Pair = pair;
                    ce.Result = res;
                    Collisions.Add(ce);
                    /*
                    //other.Velocity = Vector3.Zero;
                    other.Force = Vector3.Zero;
                  //  node.Velocity = Vector3.Zero;

                    node.Force = Vector3.Zero;
                    

                    node.Node.AddBBLines(new Vector4(0, 1, 1, 1));
                    other.Node.AddBBLines(new Vector4(0, 1, 1, 1));

                    var lf = other.Velocity;
                    var lf2 = node.Velocity;
                    //lf.X = -lf.X * 150;
                    //lf.Z = -lf.Z * 150;

                    //if (Vector3.DistanceSquared(Vector3.Zero,lf) > 0.1f)
                    //{
                        var rv = System.Numerics.Vector3.Reflect(new System.Numerics.Vector3(lf.X, lf.Y, lf.Z),new System.Numerics.Vector3(res.CollisionNormal.X, res.CollisionNormal.Y, res.CollisionNormal.Z));
                        var rv2 = System.Numerics.Vector3.Reflect(new System.Numerics.Vector3(lf2.X, lf2.Y, lf2.Z),- new System.Numerics.Vector3(res.CollisionNormal.X, res.CollisionNormal.Y, res.CollisionNormal.Z));

                    //node.Velocity = -new Vector3(rv2.X, rv2.Y, rv2.Z) * 0.7f;

                

                    other.Velocity = new Vector3(rv.X, rv.Y, rv.Z) * 0.7f;
                    //    other.Force = new Vector3(rv.X, rv.Y, rv.Z) * 70;

                        //other.Force = 
                    //
                   // else
                    {
                     //   other.Asleep = true;
                    }

                    //other.Force = lf * 150;

                    //node.Force = -lf * 150;

                    node.Position = node.LastPosition;
                    other.Position = other.LastPosition;
                    node.Node.LocalPosition = node.LastPosition;
                    other.Node.LocalPosition = other.LastPosition;
                    */
                }
                else
                {
                    //node.Node.AddBBLines(new Vector4(1, 1, 0, 1));
                    //other.Node.AddBBLines(new Vector4(1, 1, 0, 1));
                }

            }

            foreach(var collision in Collisions)
            {
                int bb = 0;

                Vector3 dir = collision.Result.Direction.Normalized();
                Vector3 other_dir = -dir;// dir.Reflect(collision.Result.Node2.Velocity.Normalized());
                collision.Result.Node1.Velocity = collision.Result.Node1.Velocity.Reflect(other_dir) * 0.8f;
                collision.Result.Node2.Velocity = collision.Result.Node2.Velocity.Reflect(dir) * 0.8f;

                /*
                var r1 = new System.Numerics.Vector3(collision.Pair.A.Velocity.X, collision.Pair.A.Velocity.Y, collision.Pair.A.Velocity.Z);
                var r2 = new System.Numerics.Vector3(collision.Pair.B.Velocity.X, collision.Pair.B.Velocity.Y, collision.Pair.B.Velocity.Z);

                Vector3 direction = collision.Result.Direction.Normalized();
                System.Numerics.Vector3 rd = new System.Numerics.Vector3(direction.X, direction.Y, direction.Z);
                
                var other_dir = System.Numerics.Vector3.Reflect(rd, r1);
                var other_Direction = new Vector3(other_dir.X, other_dir.Y, other_dir.Z);


                var d1 = new System.Numerics.Vector3(direction.X, direction.Y, direction.Z);

             

                var rv = System.Numerics.Vector3.Reflect(r1, other_dir) ;
                var rv2 = System.Numerics.Vector3.Reflect(r2,d1);
                


                collision.Result.Node1.Velocity = new Vector3(rv.X, rv.Y, rv.Z);// -collision.Result.Node1.Velocity * collision.Result.Node1.Material.Bounce * Math.Abs(collision.Result.Distance*100);
                collision.Result.Node2.Velocity = new Vector3(rv2.X,rv2.Y,rv2.Z);// -collision.Result.Node2.Velocity * collision.Result.Node2.Material.Bounce * Math.Abs(collision.Result.Distance*100);
                */
            }


        }

        

        


        


    }
}
