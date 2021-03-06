using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{

    public delegate void ButtonClicked(int button);
    public delegate void ButtonDragged(int x, int y);

    public class IButton : IForm
    {
        //bool drag = false;
        public event ButtonDragged Dragged;
        public event ButtonClicked Click = null;
        public event ButtonClicked DoubleClick = null;

        public Q.Texture.Texture2D Icon
        {
            get;
            set;
        }
        public IButton()
        {
            Icon = null;
        }
        public override void RenderForm()
        {

            if (Icon != null)
            {
                DrawButtonNoText();
                Draw(Icon, RenderPosition.X+Size.X/2-11, RenderPosition.Y+Size.Y/2-11,22,22, Color);
            }
            else
            {
                DrawFrame();
                DrawText(Text, RenderPosition.X + Size.X / 2 - TextWidth(Text) / 2, RenderPosition.Y + Size.Y / 2 - TextHeight(Text) / 2, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
                //DrawButton(Text);
            }

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

            if (Drag)
            {
                Dragged?.Invoke(x_delta, y_delta);
            }
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
            Drag = true;
            //drag = true;
        }

        public override void OnMouseUp(int button)
        {
            Color = new OpenTK.Mathematics.Vector4(1, 1, 1, 1);
            Drag = false;
            //base.OnMouseUp(button);
        }

    }
}
