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

          
        }

        public override Collision CollideWith(PhysicsNode node2)
        {


            return null;
            //return base.CollideWith(node2);

        }


    }
}
