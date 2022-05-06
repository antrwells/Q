using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class ILabel : IForm
    {
        
        public ILabel()
        {
            Text = "None";
        }

        public override void RenderForm()
        {
            base.RenderForm();
            DrawText(Text, RenderPosition.X, RenderPosition.Y,Color);
        }



    }
}
