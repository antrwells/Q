using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Harmony
{
    public class PhysicsScene
    {

        public List<PhysicsNode> Nodes
        {
            get;
            set;
        }

        public PhysicsScene()
        {

            Nodes = new List<PhysicsNode>();

        }

        public void Add(PhysicsNode node)
        {
            Nodes.Add(node);
        }

        public void Update(float time_delta)
        {

        }

    }
}
