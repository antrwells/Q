using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._2D
{
    public class EColorLimit : Shader.Effect
    {
        public EColorLimit() : base("engine/shader/colorLimitVS.glsl", "engine/shader/colorLimitFS.glsl")
        {

        }
    }
}
