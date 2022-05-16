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
        RenderTarget.RenderTargetTex2D fb2;

        public PPEmissionGlow(Scene.SceneGraph graph) : base(graph)
        {
            fb1 = GenFB(App.AppInfo.Width,App.AppInfo.Height );
            fb2 = GenFB(App.AppInfo.Width, App.AppInfo.Height);
        }

        public override Texture2D Process(Texture2D frame)
        {
            //return base.Process(frame);
            
            fb1.Bind();
            Graph.RenderEmissive();
            fb1.Release();
            fb2.Bind();
            Draw.RectBlur(0, App.AppInfo.FrameHeight, App.AppInfo.FrameWidth, -App.AppInfo.FrameHeight, fb1.BB, new OpenTK.Mathematics.Vector4(1, 1, 1, 1),0.0002f);
            fb2.Release();

            return fb2.BB;
        }

    }
}
