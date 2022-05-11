using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.RenderTarget;
namespace Q.Quantum.Forms
{
    public delegate void RenderToTarget(Vector2i size);
    public class IRenderTarget : IForm
    {

        public event RenderToTarget OnRender;
        public RenderTarget.RenderTargetTex2D Target { get; set; }

        private Texture.Texture2D tex;

        public IRenderTarget()
        {
            tex = new Texture.Texture2D("Data/test1.jpg");
        }

        public override void OnResized()
        {
            Console.WriteLine("Rezied!!!!!!!!!!!!!");
            SetSizeUnsafe(Root.Size);

        //    base.OnResized();
            if(Size.X>0 && Size.Y > 0)
            {
                if (Target == null)
                {
                    Target = new RenderTarget.RenderTargetTex2D(Size.X, Size.Y);

                }
                else 
                if (Target.BB.Width != Size.X || Target.BB.Height != Size.Y)
                {
                    if (Target != null)
                    {

                        Target.Delete();


                    }

                    Target = new RenderTarget.RenderTargetTex2D(Size.X, Size.Y);
                }
            }
        }

        public override void PreRender()
        {
            //base.PreRender();
            Target.Bind();
            UserInterface.Draw.Rect(20, 20, 200, 200, tex, new Vector4(1,1,1, 1));
            Target.Release();
            
        }

        public override void RenderForm()
        {
            //base.RenderForm();

            Draw(Target.BB, RenderPosition.X, RenderPosition.Y + Size.Y, Size.X, -Size.Y,new Vector4(1,1,1,1));

        }

    }
}
