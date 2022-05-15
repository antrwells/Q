using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._2D
{
    public class EBloom : Effect
    {

        public EBloom() : base("engine/shader/bloom2vs.glsl","engine/shader/bloom2fs.glsl")
        {
            
        }

    }
}
