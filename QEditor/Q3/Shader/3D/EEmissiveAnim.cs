using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class EEmissiveAnim  : Effect
    {
        public EEmissiveAnim() : base("engine/shader/emissiveAnimVS.glsl","engine/shader/emissiveAnimFS.glsl")
        {
            
        }
    }
}
