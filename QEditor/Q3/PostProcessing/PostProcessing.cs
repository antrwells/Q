using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.PostProcessing
{
    public class PostProcessing
    {
        public List<PostProcess> Processes = new List<PostProcess>();
        public Q.Draw.Simple.Draw2D Draw = null;
        Q.Texture.Texture2D tex1;
        GLState DrawState;

        public void SetState(GLState state)
        {
            DrawState = state;
        }

        public PostProcessing()
        {
            Draw = new Draw.Simple.Draw2D();
            tex1 = new Texture.Texture2D("data/test2.jpg");
        }

        public void Add(PostProcess process)
        {
            Processes.Add(process);
        }

        public void Process()
        {
            
            Draw.SetBlend(Q.Draw.Simple.Blend.None);
           
            foreach(var pp in Processes)
            {
                //DrawState.Bind();
                var op = pp.Process(null);

                Draw.DrawState.Blend = true;
                Draw.DrawState.BlendMode = BlendFunc.Additive;

                if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.V))
                {

                  Draw.Rect(0, App.AppInfo.Height, App.AppInfo.Width, -App.AppInfo.Height, op, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
                }
            

                
            }
            
        }

    }
}
