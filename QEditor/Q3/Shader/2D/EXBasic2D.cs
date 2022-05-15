using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._2D
{
    public class EXBasic2D : Effect
    {

        public EXBasic2D() : base("engine/shader/drawVS1.glsl","engine/shader/drawFS1.glsl")
        {
            
        }
       public override void BindPars()
        {
            //base.BindPars();
           // SetUniform("image", 0);
        }

    }
}
