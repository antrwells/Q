using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Q.Global;

namespace Q.Scene.Nodes
{
    public class SceneLight : SceneNode
    {

        public static SceneLight Active
        {
            get;
            set;
        }
        
        public Vector3 Diffuse
        {
            get;
            set;
        }

        public Vector3 Specular
        {
            get;
            set;
        }

        public float Shine
        {
            get;
            set;
        }

        public float Range
        {
            get;
            set;
        }

        public RenderTarget.RenderTargetCube ShadowFB = null;
        

        public SceneLight()
        {
            Diffuse = new Vector3(1, 1, 1);
            Specular = new Vector3(1, 1, 1);
            Shine = 1.0f;
            Range = 250.0f;
            ShadowFB = new RenderTarget.RenderTargetCube(Quality.ShadowMapWidth, Quality.ShadowMapHeight);
        }

        public void DrawShadowMap(SceneGraph graph)
        {
            Active = this;

            SceneCamera cam = new SceneCamera();
            //Effect.FXG.Cam = cam;
            cam.FOV = 90;
            cam.MaxDepth = Quality.ShadowDepth;

            //graph.CamOverride = cam;
            SceneGlobal.ActiveCamera = cam;

            cam.LocalPosition = LocalPosition;
           // cam.MaxZ = Quality.ShadowDepth;
            // cam.LocalTurn = LocalTurn;

            int fn = 0;

            GL.Disable(EnableCap.ScissorTest);
                        //App.SetVP.Set(0, 0, App.AppInfo.W, App.AppInfo.H);


            TextureTarget f = ShadowFB.SetFace(fn);

            SetCam(f, cam);

           // graph.RenderingShadows = true;

            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(1), cam);
            graph.RenderDepth();

            // ShadowFB.Release(); graph.CamOverride = null;

            SetCam(ShadowFB.SetFace(2), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(3), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(4), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(5), cam);
            graph.RenderDepth();

            ShadowFB.Release();


            SceneGlobal.ActiveCamera = graph.Camera;

            //graph.RenderingShadows = false;

            
            //graph.CamOverride = null;
        }
        private static void SetCam(TextureTarget f, SceneCamera Cam)
        {
            switch (f)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    Cam.LookAtZero(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeX:
                    Cam.LookAtZero(new Vector3(-1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapPositiveY:

                    Cam.LookAtZero(new Vector3(0, -1, 0), new Vector3(0, 0, -1));
                    break;

                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
                    break;

                case TextureTarget.TextureCubeMapPositiveZ:
                    Cam.LookAtZero(new Vector3(0, 0, 1), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeZ:
                    Cam.LookAtZero(new Vector3(0, 0, -1), new Vector3(0, -1, 0));
                    break;
            }
        }

    }
}
