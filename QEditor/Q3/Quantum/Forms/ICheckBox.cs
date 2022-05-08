using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public delegate void CheckChanged(bool check);
    
    public class ICheckBox  : IForm 
    {

        public event CheckChanged OnCheckChanged;

        public bool Checked
        {
            get;
            set;
        }

        public ICheckBox()
        {
            Checked = false;
        }
        public override void RenderForm()
        {
            base.RenderForm();

            DrawFrameRounded(RenderPosition.X, RenderPosition.Y, 16, 16, new OpenTK.Mathematics.Vector4(0.7f, 0.7f, 0.7f, 1.0f));
            if (Checked)
            {
                DrawFrameRounded(RenderPosition.X + 2, RenderPosition.Y + 2, 12, 12, new OpenTK.Mathematics.Vector4(1.4f, 1.4f, 1.4f, 1.0f));
            }
            DrawText(Text, RenderPosition.X + 22, RenderPosition.Y + 2, UserInterface.ActiveInterface.Theme.SystemTextColor);
            Size = new OpenTK.Mathematics.Vector2i(22 + TextWidth(Text), 22);

        }

        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            Checked = Checked ? false : true;
            OnCheckChanged?.Invoke(Checked);
        }

    }
}
