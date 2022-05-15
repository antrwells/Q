using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Scene;
using Q.PostProcessing;
using Q.PostProcessing.PP;

namespace Q.Renderer
{
    public class Renderer
    {

        private GLState FirstLight, SecondLight;
        private GLState ShadowState;

        private PostProcessing.PostProcessing PP = null;

        Draw.Simple.Draw2D draw = null;
        
        public Q.Scene.SceneGraph RenderGraph
        {
            get;
            set;
        }

        public Renderer()
        {
            FirstLight = new GLState();
            SecondLight = new GLState();
            ShadowState = new GLState();

            FirstLight.Blend = true;
            FirstLight.BlendMode = BlendFunc.Alpha;
            FirstLight.DepthTest = true;
            FirstLight.DepthMode = DepthFunc.LEqual;
            FirstLight.CullFace = true;
            FirstLight.CullFunc = CullMode.Back;
            FirstLight.SetViewport(0, 0, App.AppInfo.Width, App.AppInfo.Height);

            SecondLight.Blend = true;
            SecondLight.BlendMode = BlendFunc.Additive;
            SecondLight.DepthTest = true;
            SecondLight.DepthMode = DepthFunc.Equal;
            SecondLight.CullFace = true;
            SecondLight.CullFunc = CullMode.Back;
            SecondLight.SetViewport(0, 0, App.AppInfo.Width, App.AppInfo.Height);

            ShadowState.Blend = false;
            ShadowState.DepthTest = true;
            ShadowState.DepthMode = DepthFunc.LEqual;
            ShadowState.CullFace = true;
            ShadowState.CullFunc = CullMode.Back;
            //hadowState.SetViewport(

            draw = new Draw.Simple.Draw2D();
            tex1 = new Texture.Texture2D("Data/test2.jpg");

            PP = new PostProcessing.PostProcessing();

            

        }
        Q.Texture.Texture2D tex1;
        public void SetGraph(SceneGraph graph)
        {
            RenderGraph = graph;
            var emGlow = new PostProcessing.PP.PPEmissionGlow(graph);

            PP.Add(emGlow);
            PP.SetState(FirstLight);
        }

        public void RenderScene()
        {
            OpenTK.Graphics.OpenGL.GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);

            ShadowState.Bind();

            RenderGraph.RenderShadows();

          //  FirstLight.Bind();

          
          
            SceneGlobal.ActiveCamera = RenderGraph.Camera;
            FirstLight.Bind();
            foreach (var light in RenderGraph.Lights)
            {
                SceneGlobal.ActiveLight = light;
                RenderGraph.RenderGraph();

               SecondLight.Bind();

            }

            PP.Process();
            

            //draw.Rect(20, 20, 500, 500, tex1, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));


        }

    }
}
