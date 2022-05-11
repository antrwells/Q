using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class IGroup : IForm
    {
        public override void RenderForm()
        {
            base.RenderForm();
            DrawOutline(new OpenTK.Mathematics.Vector4(1, 1, 1, 1)) ;
        }
    }
}
