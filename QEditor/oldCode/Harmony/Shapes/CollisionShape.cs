using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Harmony.Shapes
{
    public class CollisionShape
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

        public PhysicsNode Node
        {
            get;
            set;
        }

        public virtual void SetToBounds(Q.Scene.Nodes.SceneNode node)
        {
            
        }

        public virtual Q.Scene.Nodes.BoundingBox GetBounds()
        {
            return null;
        }

    }
}
