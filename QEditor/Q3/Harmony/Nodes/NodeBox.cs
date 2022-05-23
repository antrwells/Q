using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Harmony.Nodes
{
    public class NodeBox : PhysicsNode
    {

        public Vector3 Size
        {
            get;
            set;
        }

        public NodeBox()
        {
            Size = new Vector3(1, 1, 1);
        } 

        public NodeBox(Scene.Nodes.SceneNode node)
        {

            var bb = node.GetBounds();

            var size = new Vector3();

            size.X = bb.Max.X - bb.Min.X;
            size.Y = bb.Max.Y - bb.Min.Y;
            size.Z = bb.Max.Z - bb.Min.Z;

            Node = node;

            Size = size;

            float m = Mass;
            float w = size.X;
            float h = size.Y;
            float d = size.Z;
            MomentOfInertia = m * (w * w + h * h + d * d) / 12.0f;

        }

        public override Collision CollideWith(PhysicsNode node2)
        {


            return null;
            //return base.CollideWith(node2);

        }


    }
}
