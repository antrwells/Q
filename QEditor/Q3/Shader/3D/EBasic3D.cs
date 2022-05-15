using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class EBasic3D : Effect
    {
        public EBasic3D() : base("engine/shader/vsColorOnly.glsl","engine/shader/fsColorOnly.glsl")
        {
            
        }

        public override void BindPars()
        {
            //base.BindPars();
           // SetUniform("image", 0);
        }

    }
}
