using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Scene.Nodes;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Q.Scene
{
    public class SceneGraph
    {

        public SceneNode Root
        {
            get;
            set;
        }

        public SceneCamera Camera
        {
            get;
            set;
        }

        public List<SceneLight> Lights
        {
            get;
            set;
        }

        public SceneGraph()
        {

            Root = new SceneNode();
            Camera = new SceneCamera();
            Lights = new List<SceneLight>();

        }

        public void Add(SceneNode node)
        {
            Root.Add(node);
        }

        public void Add(SceneLight light)
        {
            Lights.Add(light);
        }

        public void SetCamera(SceneCamera camera)
        {
            Camera = camera;
        }

        public void RenderGraph()
        {
            Root.Render();
        }
    
        public void RenderGraph2()
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            SceneGlobal.ActiveCamera = Camera;
            //GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            foreach (var light in Lights)
            {
                
                SceneGlobal.ActiveLight = light;
                Root.Render();

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                GL.DepthFunc(DepthFunction.Equal);

            }


        }
        public void RenderEmissive()
        {

            //GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Disable(EnableCap.CullFace);

            GL.Disable(EnableCap.Blend);
            Root.RenderEmissive();            
        }
        public void RenderDepth()
        {
            GL.Disable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Lequal);
            Root.RenderDepth();
            
        }

        public void RenderShadows()
        {
            int ls = 0;
         //   GL.Disable(EnableCap.Blend);
           // GL.Disable(EnableCap.DepthTest);
         //   GL.Disable(EnableCap.CullFace);
            
            foreach (SceneLight l in Lights)
            {
                ls++;
                l.DrawShadowMap(this);
                // Console.WriteLine("LightShadows:" + ls);
            }
        }

    }
}
