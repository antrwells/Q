using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{

    public delegate void ButtonClicked(int button);

    public class IButton : IForm
    {
        bool drag = false;

        public ButtonClicked Click = null;
        public ButtonClicked DoubleClick = null;

        public override void RenderForm()
        {

            DrawButton(Text);

        }

        public override void OnEnter()
        {
            Color = new OpenTK.Mathematics.Vector4(1.2f,1.2f,1.2f, 1);
            //base.OnEnter();

        }

        public override void OnLeave()
        {
            Color = new OpenTK.Mathematics.Vector4(1, 1, 1, 1);
            //
            //base.OnLeave();
        }

        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
           
        }

        public override void OnDoubleClick(int button)
        {
            //base.OnDoubleClick(button);
            DoubleClick?.Invoke(button);


        }

        public override void OnMouseDown(int button)
        {
            Color = new OpenTK.Mathematics.Vector4(1.3f, 1, 1, 1);
            Click?.Invoke(button);
            drag = true;
        }

        public override void OnMouseUp(int button)
        {
            Color = new OpenTK.Mathematics.Vector4(1, 1, 1, 1);
            drag = false;
            base.OnMouseUp(button);
        }

    }
}
