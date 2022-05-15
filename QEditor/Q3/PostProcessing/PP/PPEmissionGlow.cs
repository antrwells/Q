using Q.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.PostProcessing.PP
{
    public class PPEmissionGlow : PostProcess
    {

        RenderTarget.RenderTargetTex2D fb1;

        public PPEmissionGlow(Scene.SceneGraph graph) : base(graph)
        {
            fb1 = GenFB(App.AppInfo.Width / 2, App.AppInfo.Height / 2);
        }

        public override Texture2D Process(Texture2D frame)
        {
            //return base.Process(frame);
          
            fb1.Bind();
            Graph.RenderEmissive();
            fb1.Release();
            return fb1.BB;
        }

    }
}
