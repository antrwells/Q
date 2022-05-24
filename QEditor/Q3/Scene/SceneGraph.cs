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
        private GLState FirstLight, SecondLight;
        private GLState ShadowState;
        public SceneGraph()
        {

            Root = new SceneNode();
            Camera = new SceneCamera();
            Lights = new List<SceneLight>();
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
            SceneGlobal.ActiveCamera = Camera;
            FirstLight.Bind();
            foreach (var light in Lights)
            {
                SceneGlobal.ActiveLight = light;
                Root.Render();

                SecondLight.Bind();

            }
            //Root.Render();
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

            FirstLight.Bind();
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
            FirstLight.Bind();
            
            foreach (SceneLight l in Lights)
            {
                ls++;
                l.DrawShadowMap(this);
                // Console.WriteLine("LightShadows:" + ls);
            }
        }

        public void Update()
        {
            UpdateNode(Root);
        }

        public void UpdateNode(SceneNode node)
        {

            node.Update();
            foreach(var cnode in node.Child)
            {
                UpdateNode(cnode);
            }

        }

        public void Remove(SceneNode node)
        {

            RemoveNode(Root, node);


        }

        public void RemoveNode(SceneNode from,SceneNode node)
        {

            if (from.Child.Contains(node)){
                from.Child.Remove(node);
                return;
            }

            foreach(var cnode in from.Child.ToArray())
            {
                RemoveNode(cnode, node);
            }



        }


    }
}
