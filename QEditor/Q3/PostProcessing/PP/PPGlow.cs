using Q.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.PostProcessing.PP
{
    public class PPGlow : PostProcess
    {

        RenderTarget.RenderTargetTex2D fb1, fb2;

        public PPGlow(Q.Scene.SceneGraph graph) : base(graph)
        {
            fb1 = GenFB(App.AppInfo.Width , App.AppInfo.Height );
            fb2 = GenFB(App.AppInfo.Width , App.AppInfo.Height );
        }

        public override Texture2D Process(Texture2D frame)
        {
            //return base.Process(frame);
            fb1.Bind();
            Graph.RenderGraph();
            fb1.Release();

            Q.Draw.Simple.SetFXPars pars = () =>
            {
                Q.Global.GlobalEffects.ColorLimitFX.SetUniform("limit", 0.4f);
            };

            fb2.Bind();
            Draw.RectFX(Q.Global.GlobalEffects.ColorLimitFX, 0, Q.App.AppInfo.FrameHeight, Q.App.AppInfo.FrameWidth, -Q.App.AppInfo.FrameHeight, fb1.BB, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), pars);
            fb2.Release();

            fb1.Bind();
            Draw.RectBlur(0, Q.App.AppInfo.Height, Q.App.AppInfo.Width, -Q.App.AppInfo.Height, fb2.BB, new OpenTK.Mathematics.Vector4(1, 1, 1, 1),0.8f);
            fb1.Release();


            Q.Draw.Simple.SetFXPars bpars = () =>
            {
            //    Q.Global.GlobalEffects.ColorLimitFX.SetUniform("", 0.2f);
            };

            Graph.RenderGraph();

            Draw.SetBlend(Q.Draw.Simple.Blend.Additive);
            Draw.RectFX(Q.Global.GlobalEffects.BloomFX, 0, Q.App.AppInfo.FrameHeight, Q.App.AppInfo.FrameWidth, -Q.App.AppInfo.FrameHeight, fb1.BB, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), bpars);
            Draw.SetBlend(Q.Draw.Simple.Blend.None);

            return fb1.BB;

            
        }


    }
}
