using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class IGroup : IForm
    {
        public bool DisplayFrame
        {
            get;
            set;
        }
        public IGroup()
        {
            DisplayFrame = true;
        }

        public override void RenderForm()
        {
            base.RenderForm();
            if (DisplayFrame)
            {
                DrawFrame();
                DrawOutline(new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            }
        }
    }
}
