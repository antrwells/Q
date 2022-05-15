using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class EEmissive3D : Effect
    {
        public EEmissive3D() : base("engine/shader/vsEmissive.glsl", "engine/shader/fsEmissive.glsl")
        {

        }
    }
}
