using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._2D
{
    public class EXBasic2D : Effect
    {

        public EXBasic2D() : base("engine/shader/basic_draw_vertex.glsl","engine/shader/basic_draw_frag.glsl")
        {
            
        }
        public override void BindPars()
        {
            //base.BindPars();
            SetUniform("image", 0);
        }

    }
}
