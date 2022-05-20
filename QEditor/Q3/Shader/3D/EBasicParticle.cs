using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class EBasicParticle : Effect
    {

        public EBasicParticle() : base("engine/shader/particleVS.glsl","engine/shader/particleFS.glsl")
        {

        }

    }
}
