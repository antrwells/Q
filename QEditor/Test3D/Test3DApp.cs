using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.App;
using OpenTK.Windowing.Desktop;
using Q.Scene;
using Q.Scene.Nodes;
using Q.PostProcessing;
using Q.PostProcessing.PP;
namespace Test3D
{
    public class Test3DApp : Application
    {

        Q.Import.AssImpImport imp;
        SceneNode s1, s2;
        SceneCamera cam;
        SceneLight l1;
        SceneGraph g1;
        PostProcessing pp;
        PPEmissionGlow glow;
        Q.Draw.Simple.Draw2D draw;
        public Test3DApp(GameWindowSettings window_settings, NativeWindowSettings native_settings) : base(window_settings, native_settings)
        {

        }
        Q.Texture.Texture2D map;
        public override void InitApp()
        {
            base.InitApp();
            imp = new Q.Import.AssImpImport();
            s1 = imp.LoadNode("data/test1.fbx");
            s2 = imp.LoadNode("data/t3d/cybermale/scene.gltf");
            int a = 1;
            cam = new SceneCamera();
            cam.LocalPosition = new OpenTK.Mathematics.Vector3(0, 10, 25);
            l1 = new SceneLight();
            s1.LocalRotation = OpenTK.Mathematics.Matrix4.Identity;
            SceneGlobal.ActiveLight = l1;
            l1.LocalPosition = cam.LocalPosition;
            l1.LocalPosition = new OpenTK.Mathematics.Vector3(25, 10, 25);
            SceneLight l2 = new SceneLight();

            g1 = new SceneGraph();
            g1.Add(s1);
            g1.Add(s2);
            g1.Add(l1);
        //    g1.Add(l2);
            g1.SetCamera(cam);
            l2.LocalPosition = new OpenTK.Mathematics.Vector3(-25, 10, 25);
            l1.Diffuse = new OpenTK.Mathematics.Vector3(1, 1, 1);
            l2.Diffuse = new OpenTK.Mathematics.Vector3(1, 1, 1);
            pp = new PostProcessing();
            glow = new PPEmissionGlow(g1);
            draw = new Q.Draw.Simple.Draw2D();
            map = new Q.Texture.Texture2D("data/test2.jpg");
        }

        public override void UpdateApp()
        {
            base.UpdateApp();
        }

        float p = 0, y = 0;
        public override void RenderApp()
        {
            base.RenderApp();

            SceneGlobal.ActiveCamera = cam;
            SceneGlobal.ActiveNode = s1;
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
            {
                l1.LocalPosition = cam.LocalPosition;
            }
            if (Q.Input.AppInput.MouseButton[1])
            {
                p = p - Q.Input.AppInput.MouseDelta.Y * 0.1f;
                y = y - Q.Input.AppInput.MouseDelta.X * 0.1f;
                cam.Rotate(p, y, 0);
            }

            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
            {
                cam.Move(new OpenTK.Mathematics.Vector3(0, 0, -0.1f));
            }
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
            {
                cam.Move(new OpenTK.Mathematics.Vector3(0, 0, 0.1f));
            }
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
            {
                cam.Move(new OpenTK.Mathematics.Vector3(-0.1f, 0, 0));
            }
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
            {
                cam.Move(new OpenTK.Mathematics.Vector3(0.1f, 0, 0));
            }

            Q.Input.AppInput.MouseDelta = new OpenTK.Mathematics.Vector2(0, 0);

            //s1.RenderModules();
            //g1.RenderGraph();
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
            {
                g1.RenderEmissive();
           

            }
            else
            {
                g1.RenderShadows();
                g1.RenderGraph();
                var map = glow.Process(null);


                draw.SetBlend(Q.Draw.Simple.Blend.Additive);


                draw.RectBlur(0, Q.App.AppInfo.Height, Q.App.AppInfo.Width, -Q.App.AppInfo.Height, map, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), 0.9f);
                

            }

        }

    }
}
