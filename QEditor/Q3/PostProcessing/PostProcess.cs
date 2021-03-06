using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.PostProcessing
{
    public class PostProcess
    {
        public Scene.SceneGraph Graph = null;
        public Draw.Simple.Draw2D Draw;

        public PostProcess(Scene.SceneGraph graph)
        {
            Graph = graph;
            Draw = new Draw.Simple.Draw2D();
            Draw.SetBlend(Q.Draw.Simple.Blend.None);
        }
        public virtual Texture.Texture2D Process(Texture.Texture2D frame)
        {

            return null;
        }

        public RenderTarget.RenderTargetTex2D GenFB(int w=-1,int h=-1)
        {
            if (w == -1)
            {
                w = App.AppInfo.Width;
                h = App.AppInfo.Height;
            }
            return new RenderTarget.RenderTargetTex2D(w, h);
        }
            
    }
}
