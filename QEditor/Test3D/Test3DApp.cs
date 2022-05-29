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
using Q.Elemental;
using Q.Elemental.FX;

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
        Q.Renderer.Renderer mRender;
        PPEmissionGlow emGlow;
        PPGlow glow;
        Q.Draw.Simple.Draw2D draw;
        Elemental efx;
        ParticleFX fx1;
        Particle part1;
        Particle part2;


        
        
        public Test3DApp(GameWindowSettings window_settings, NativeWindowSettings native_settings) : base(window_settings, native_settings)
        {
            
        }
        Q.Texture.Texture2D map;
        public override void InitApp()
        {
            base.InitApp();
            imp = new Q.Import.AssImpImport();
            s1 = imp.ImportNode("data/test1.fbx");
           s2 = imp.ImportNode("data/convex1.fbx");
            var s3 = imp.ImportNode("data/convex1.fbx");

            s1.Child[0].SetPhysicsBodyType(Q.Physx.BodyType.TriMesh);
            s2.Child[0].SetPhysicsBodyType(Q.Physx.BodyType.ConvexHull);
            s3.Child[0].SetPhysicsBodyType(Q.Physx.BodyType.ConvexHull);

            //s1.Child[0].SetPhysicsTriMesh();
            //s2.Child[0].SetPhysicsSphere();
            //s3.Child[0].SetPhysicsSphere();

            //s2.LocalScale = new OpenTK.Mathematics.Vector3(0.02f, 0.02f, 0.02f);
            //Q.Anim.ActorAnim anim1 = new Q.Anim.ActorAnim("Walk", 0, 82, 0.5f, Q.Anim.AnimType.Forward);
            //s2.Animations.Add(anim1);
            //s2.PlayAnim("Walk");
            // s2.CurrentAnim = anim1;
            //s1.Child[0].XBody.MakeStatic();


          




           
           // p3.Force = new OpenTK.Mathematics.Vector3(-3, 0, 0);
            efx = new Elemental();
            fx1 = new ParticleFX();
            part1 = new Particle();
            part2 = new Particle();
            efx.Add(fx1);
            fx1.Position = new OpenTK.Mathematics.Vector3(0, 2, 0);

          
        //    s2.LocalPosition = new OpenTK.Mathematics.Vector3(0, 1, 0);
            int a = 1;
      //     s2.LocalPosition = new OpenTK.Mathematics.Vector3(0, 2, 0);
            cam = new SceneCamera();
            cam.LocalPosition = new OpenTK.Mathematics.Vector3(0, 10, 25);
            l1 = new SceneLight();
            s1.LocalRotation = OpenTK.Mathematics.Matrix4.Identity;
            SceneGlobal.ActiveLight = l1;
            l1.LocalPosition = cam.LocalPosition;
            l1.LocalPosition = new OpenTK.Mathematics.Vector3(25, 10, 25);
            SceneLight l2 = new SceneLight();
            s1.AddBBLines(new OpenTK.Mathematics.Vector4(1,1,0,1));
            g1 = new SceneGraph();
            g1.Add(s1);
            g1.Add(s2);
            g1.Add(l1);
            g1.Add(s3);

           g1.Add(l2);
            s2.Child[0].LocalPosition = new OpenTK.Mathematics.Vector3(0, 15, 3);
            s2.Child[0].Rotate(45, 45, 45);
            s3.Child[0].LocalPosition = new OpenTK.Mathematics.Vector3(0, 20, 0);
            s1.Child[0].XBody.MakeStatic();
            g1.SetCamera(cam);
            l2.LocalPosition = new OpenTK.Mathematics.Vector3(-25, 10, 25);
            l1.Diffuse = new OpenTK.Mathematics.Vector3(2, 1, 1);
            l2.Diffuse = new OpenTK.Mathematics.Vector3(1,2, 2);
            pp = new PostProcessing();
            emGlow = new PPEmissionGlow(g1);
            glow = new PPGlow(g1);
            pp.Add(emGlow);
            pp.Add(glow);
            draw = new Q.Draw.Simple.Draw2D();
            map = new Q.Texture.Texture2D("data/test2.jpg");
            mRender = new Q.Renderer.Renderer();
            mRender.SetGraph(g1);


            part1.Image = new Q.Texture.Texture2D("data/fire1.png");


            fx1.SpawnPosMin = new OpenTK.Mathematics.Vector3(-0.1f, -0.1f, -0.1f);
            fx1.SpawnPosMax = new OpenTK.Mathematics.Vector3(0.1f,0.1f,0.1f);
            fx1.SpawnInertiaMin = new OpenTK.Mathematics.Vector3(-0.02f, 0.01f, -0.02f);
            fx1.SpawnInertiaMax = new OpenTK.Mathematics.Vector3(0.02f,0.03f,0.02f);
            efx.CurrentScene = g1;

            //       p2.Static = true;

           //s2.AddBBLines(new OpenTK.Mathematics.Vector4(1, 1, 0, 1));

            //s2.Meshes[0].Material.ColorMap = new Q.Texture.Texture2D("data/Vampire_Diffuse.png");
           // s2.Meshes[0].Material.NormalMap = new Q.Texture.Texture2D("data/norm_Vampire_Diffuse.png");
         //   s2.Meshes[0].Material.SpecularMap = new Q.Texture.Texture2D("data/spec_Vampire_Diffuse.png");
       //     s2.Meshes[0].Material.EmissiveMap = new Q.Texture.Texture2D("data/vampire_emission.png");
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

            //if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
            //{
            //   s2.Update();
            // emGlow.Glow = !emGlow.Glow;
            // }

            //s2.Update();
            //   s2.AlwaysFaceCamera = true;

          //  p2.ApplyTorque(200,0, 0);


            //s1.RenderModules();
            g1.RenderShadows();

        
                g1.RenderGraph();
                //pp.Process();

            g1.Update();
      

            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Z))
            {
                fx1.Spawn(part1, 20);
            }

            efx.Update();



            //mRender.RenderScene();
            
            //g1.RenderGraph();
            /*
            if (Q.Input.AppInput.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
            {
                g1.RenderShadows();
                g1.RenderGraph();
            //    g1.RenderEmissive();

           

            }
            else
            {
                g1.RenderShadows();
                //  g1.RenderGraph();
                //return;
                                //g1.RenderEmissive();

                pp.Process();


               // var map = glow.Process(null);


                draw.SetBlend(Q.Draw.Simple.Blend.None);


             //   draw.Rect(0, Q.App.AppInfo.Height, Q.App.AppInfo.Width, -Q.App.AppInfo.Height, map, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
                

            }
            */

        }

    }
}
