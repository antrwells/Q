using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class EDepthAnimMesh : Effect
    {

        public EDepthAnimMesh() : base("engine/shader/depthAnimVS.glsl","engine/shader/depthFS.glsl")
        {
            
        }

    }
}
