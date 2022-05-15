using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Scene
{
    public class NodeModule
    {
        
        public Scene.Nodes.SceneNode Node
        {
            get;
            set;
        }

        public virtual void InitModule()
        {

        }

        public virtual void UpdateModule()
        {

        }

        public virtual void RenderModule()
        {
            
        }
    
    }
}
