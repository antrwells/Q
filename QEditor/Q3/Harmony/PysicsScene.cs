using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
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

        public PhysicsScene()
        {

            Nodes = new List<PhysicsNode>();
            Gravity = new Vector3(0, -0.2f, 0);
           // Gravity = new Vector3(0, 0, 0);

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

        public void Update(float time_delta)
        {

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
                    //other.Velocity = Vector3.Zero;
                    other.Force = Vector3.Zero;
                  //  node.Velocity = Vector3.Zero;

                    node.Force = Vector3.Zero;
                    

                    node.Node.AddBBLines(new Vector4(0, 1, 1, 1));
                    other.Node.AddBBLines(new Vector4(0, 1, 1, 1));

                    var lf = other.LastPosition - other.Position;
                    //lf.X = -lf.X * 150;
                    //lf.Z = -lf.Z * 150;

                    other.Force = lf * 150;
                    //node.Force = -lf * 150;

                    node.Position = node.LastPosition;
                    other.Position = other.LastPosition;
                    node.Node.LocalPosition = node.LastPosition;
                    other.Node.LocalPosition = other.LastPosition;
                }
                else
                {
                    node.Node.AddBBLines(new Vector4(1, 1, 0, 1));
                    other.Node.AddBBLines(new Vector4(1, 1, 0, 1));
                }

            }


        }

        

        


        


    }
}
