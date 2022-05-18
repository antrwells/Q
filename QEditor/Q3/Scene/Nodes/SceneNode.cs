using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Scene.Nodes
{
    public enum NodeType
    {
        Entity,Actor
    }
    public class SceneNode
    {

        public SceneNode Root
        {
            get;
            set;
        }

        public NodeType Type
        {
            get;
            set;
        }

        public List<SceneNode> Child
        {
            get;
            set;
        }


        public List<NodeModule> Modules
        {
            get;
            set;
        }

        public Dictionary<string, Q.Anim.BoneInfo> m_BoneInfoMap
        {
            get;
            set;
        }

        public int m_BoneCount
        {
            get;
            set;
        }

        public static Shader.Effect MeshFX = new Q.Shader._3D.EBasic3D();


        public List<Mesh.Mesh3D> Meshes = new List<Mesh.Mesh3D>();

        public Anim.Animator Animator
        {
            get;
            set;
        }

        public Q.Anim.ActorAnim CurrentAnim
        {
            get;
            set;
        }

        public List<Q.Anim.ActorAnim> Animations = new List<Anim.ActorAnim>();

        public void SetBoneInfoMap(Dictionary<string, Q.Anim.BoneInfo> boneInfoMap, int count)
        {
            m_BoneInfoMap = boneInfoMap;
            m_BoneCount = count;
        }

        //Transform

        public virtual Matrix4 WorldMatrix
        {
            get
            {
                Matrix4 r = Matrix4.Identity;

                if (Root != null)
                {
                    r = Root.WorldMatrix;
                }

                r = (Matrix4.CreateScale(LocalScale) * LocalRotation * Matrix4.CreateTranslation(LocalPosition)) * r;


                return r;
            }
        }

        public virtual Matrix4 WorldMatrixNormal
        {
            get
            {
                Matrix4 r = Matrix4.Identity;


                r = LocalRotation * Matrix4.CreateTranslation(LocalPosition); ;

                return r;
            }
        }


        public Matrix4 LocalRotation
        {
            get;
            set;
        }

        public Vector3 LocalPosition
        {
            get;
            set;
        }

        public Vector3 LocalScale
        {
            get;
            set;
        }

        public Matrix4 GlobalInverse
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public NodeModule GetModule<T>() where T : NodeModule
        {

            foreach (var module in Modules)
            {
                if (module is T)
                {
                    return module;
                }
            }
            return null;
        }

        public SceneNode()
        {
            Root = null;
            Child = new List<SceneNode>();
            Modules = new List<NodeModule>();
            LocalRotation = Matrix4.Identity;
            LocalPosition = Vector3.Zero;
            LocalScale = Vector3.One;
            if (MeshFX == null)
            {
                MeshFX = new Shader._3D.EBasic3D();
            }
        }

        public void Update()
        {
            if (CurrentAnim == null)
            {
                return;
            }
            CurrentAnim.Update();
            Animator.SetTime(CurrentAnim.CurTime);


        }

        public virtual void Rotate(float pitch, float yaw, float roll = 0)
        {

        }

        public void Move(Vector3 v)
        {
            // v.X = -v.X;
            //if (s == Space.Local)

            //Console.WriteLine("NV:" + v);
            Vector3 ov = LocalPosition; ;
            Vector3 nv = Vector3.TransformPosition(v, WorldMatrixNormal);
            Vector3 mm = nv - ov;


            LocalPosition = LocalPosition + mm;//Matrix4.Invert(nv);
                                               //LocalPos = LocalPos + new Vector3(nv.X, nv.Y, nv.Z);
                                               //}
        }

        public void Add(SceneNode node)
        {
            Child.Add(node);
            node.Root = this;
        }

        public void AddModule(NodeModule module)
        {
            Modules.Add(module);
        }

        public void AddMesh(Mesh.Mesh3D mesh)
        {
            Meshes.Add(mesh);
        }

        public void RenderModules()
        {
            foreach (var module in Modules)
            {
                module.RenderModule();
            }
            foreach (var child in Child)
            {
                //   child.RenderModules();
            }
        }
        public void RenderEmissive()
        {
            var fx = Global.GlobalEffects.EmissiveFX;
            SceneGlobal.ActiveNode = this;

            var rm = GetModule<Scene.Modules.ModuleMesh3D>() as Scene.Modules.ModuleMesh3D;
            if (rm != null)
            {
                fx.Bind();

                Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;



                fx.SetUniform("proj", pm);
                fx.SetUniform("model", mm);
                fx.SetUniform("view", vm);
                fx.SetUniform("tEmissive", 0);

                foreach (var mesh in rm.Meshes)
                {
                    if (mesh.Material.EmissiveMap != null)
                    {
                        mesh.Material.EmissiveMap.Bind(Texture.TextureUnit.Unit0);

                        mesh.DrawBindOnly();
                    }

                }
                fx.Release();
            }
            foreach (var child in Child)
            {
                child.RenderEmissive();
            }
        }
        public void RenderDepth()
        {
            SceneGlobal.ActiveNode = this;
            var rm = GetModule<Scene.Modules.ModuleMesh3D>() as Scene.Modules.ModuleMesh3D;
            if (Meshes.Count > 0)
            {

                var fx = Global.GlobalEffects.DepthFX;

                fx.Bind();

                Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

                fx.SetUniform("proj", pm);
                fx.SetUniform("model", mm);
                fx.SetUniform("view", vm);
                fx.SetUniform("camP", SceneGlobal.ActiveCamera.LocalPosition);
                fx.SetUniform("minZ", SceneGlobal.ActiveCamera.MinDepth);
                fx.SetUniform("maxZ", SceneGlobal.ActiveCamera.MaxDepth);

                //int a = 1;
                foreach (var mesh in Meshes)
                {
                    mesh.DrawBindOnly();

                }
                fx.Release();
            }
            foreach (var child in Child)
            {
                child.RenderDepth();
            }
        }
        public void Render()
        {

            switch (Type)
            {
                case NodeType.Entity:
                    SceneGlobal.ActiveNode = this;
                    foreach (var mesh in Meshes)
                    {
                        mesh.Draw(MeshFX);
                    }
                    foreach (var child in Child)
                    {
                        child.Render();
                    }
                    break;
                case NodeType.Actor:

                    var finalMatrices = Animator.GetFinalBoneMatrices();
                    SceneGlobal.ActiveNode = this;
                    var afx = Global.GlobalEffects.AnimMeshFX;

                    Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                    Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                    Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

                    var light = Scene.SceneGlobal.ActiveLight;

                    foreach (var mesh in Meshes)
                    {

                        mesh.Material.ColorMap.Bind(Texture.TextureUnit.Unit0);
                        mesh.Material.NormalMap.Bind(Texture.TextureUnit.Unit1);
                        mesh.Material.SpecularMap.Bind(Texture.TextureUnit.Unit3);
                        light.ShadowFB.Cube.Bind(2);

                        afx.Bind();
                        afx.SetUniform("mProj", pm);
                        afx.SetUniform("mModel", mm);
                        afx.SetUniform("mView", vm);
                        afx.SetUniform("viewPos", Scene.SceneGlobal.ActiveCamera.LocalPosition);
                        afx.SetUniform("tCol", 0);
                        afx.SetUniform("tNorm", 1);
                        afx.SetUniform("tSpec", 3);
                        afx.SetUniform("tShadow", 2);
                        afx.SetUniform("tEnv", 4);
                        afx.SetUniform("lightDepth", Scene.SceneGlobal.ActiveCamera.MaxDepth);
                        afx.SetUniform("light_position", light.LocalPosition);
                        afx.SetUniform("lDiff", light.Diffuse);
                        afx.SetUniform("lSpec", light.Specular);
                        afx.SetUniform("lRange", light.Range);
                        afx.SetUniform("shadowMapping", 0);
                        afx.SetUniform("envMapping", 0);
                        afx.SetUniform("refract", 0);

                        for (int i = 0; i < finalMatrices.Count; ++i)
                        {
                            afx.SetUniform("bone_transforms[" + i + "]", finalMatrices[i]);
                        }


                        mesh.DrawBindOnly();
                        afx.Release();

                    }

                    int a = 0;

                    break;
            }
            
    }

        public void LookAtZero(Vector3 p, Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalRotation = m;
        }

    }
}
