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

        public bool BlurBackground
        {
            get;
            set;
        }

        public IGroup()
        {
            DisplayFrame = true;
            BlurBackground = false;
        }

        public override void RenderForm()
        {
            base.RenderForm();
            if (DisplayFrame)
            {
                DrawFrame(new OpenTK.Mathematics.Vector4(1, 1, 1, 0.55f));
                DrawBlur(RenderPosition.X,RenderPosition.Y,Size.X,Size.Y,0.2f);
                DrawOutline(new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            }
        }
    }
}
