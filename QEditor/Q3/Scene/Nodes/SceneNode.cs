using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Physx;

namespace Q.Scene.Nodes
{

    public class BoundingBox
    {
        public Vector3 Min
        {
            get;
            set;
        }

        public Vector3 Max
        {
            get;
            set;
        }
        public override string ToString()
        {
            //return base.ToString();

            string r = "Min:";
            r = r  +Min.ToString();
            r = r + "Max:";
            r = r + Max.ToString();
            return r;

        }
    }
    public enum NodeType
    {
        Entity,Actor,Particle
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

        public static Shader.Effect MeshFX;
        public static Shader.Effect ParticleFX;


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

        public bool AlwaysFaceCamera
        {
            get;
            set;
        }

        public List<Q.Anim.ActorAnim> Animations = new List<Anim.ActorAnim>();

        public PXBody XBody
        {
            get;
            set;
        }

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

                if (Root != null)
                {
                    r = Root.WorldMatrixNormal;
                }

                r =  (LocalRotation * Matrix4.CreateTranslation(LocalPosition)) * r;


                return r;
            }
        }


        public Matrix4 LocalRotation
        {
            get
            {
                return _LocalRotation;
            }
            set
            {
                _LocalRotation = value;
                if (XBody != null)
                {
                    XBody.SetPose(_LocalPosition,_LocalRotation);

                //    XBodu.SetRotation(_LocalRotation);
                }
            }
        }
        private Matrix4 _LocalRotation = Matrix4.Identity;

        public Vector3 LocalPosition
        {
            get
            {
                return _LocalPosition;
            }
            set
            {
                _LocalPosition = value;
                if (XBody != null)
                {
                    //Console.WriteLine("!!");
                    XBody.SetPose(_LocalPosition, _LocalRotation);
                }
            }
        }
        private Vector3 _LocalPosition = Vector3.Zero;

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

        public Vector4 Color
        {
            get;
            set;
               
        }

        public float Spin
        {
            get;
            set;
        }


        public Mesh.MeshLines Lines
        {
            get;
            set;
        }


        public List<Mesh.MeshPoints> PointMeshes
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

        public void SetPhysicsBodyType(BodyType type, bool isStatic = false)
        {
            switch (type)
            {
                case BodyType.Box:
                    SetPhysicsBox(isStatic);
                    break;
                case BodyType.Sphere:
                    SetPhysicsSphere();
                    break;
                case BodyType.TriMesh:
                    SetPhysicsTriMesh();
                    break;
                case BodyType.ConvexHull:
                    SetPhysicsConvexHull();
                    break;

            }
        }

        public void SetPhysicsConvexHull()
        {
            XBody = new PXConvexHull(Meshes[0]);
        }
        public void SetPhysicsTriMesh()
        {

            XBody = new PXTriMesh(Meshes, 0);

        }

        public void SetPhysicsSphere()
        {

            var radius = GetRadius();
            XBody = new PXSphere(radius);


        }
        public void SetPhysicsBox(bool IsStatic)
        {
            var bounds = GetBounds();
            if (IsStatic)
            {
                XBody = new PXStaticBox(bounds.Max.X - bounds.Min.X, bounds.Max.Y - bounds.Min.Y, bounds.Max.Z - bounds.Min.Z);
            }
            else
            {
                XBody = new PXBox(bounds.Max.X - bounds.Min.X, bounds.Max.Y - bounds.Min.Y, bounds.Max.Z - bounds.Min.Z);
            }
        
        }

        public void UpdatePhysics()
        {
            if (XBody != null)
            {
                _LocalPosition = XBody.GetPos();
                _LocalRotation = XBody.GetRot();
            }
        }

        public void AddBBLines(Vector4 col)
        { 

            var bb = GetBounds();

            Lines = new Mesh.MeshLines();

            Vector3 min = bb.Min;// - LocalPosition;
            Vector3 max = bb.Max;// - LocalPosition;
            //min = LocalPosition - min;
            //max = LocalPosition  max;



            Vector3 t1, t2, t3, t4;
            Vector3 b1, b2, b3, b4;

            t1 = min;
            t2 = new Vector3(max.X, min.Y, min.Z);
            t3 = new Vector3(max.X, min.Y, max.Z);
            t4 = new Vector3(min.X, min.Y, max.Z);

            b1 = new Vector3(min.X, max.Y, min.Z);
            b2 = new Vector3(max.X, max.Y, min.Z);
            b3 = new Vector3(max.X, max.Y, max.Z);
            b4 = new Vector3(min.X, max.Y, max.Z);

            Lines.AddLine(t1, t2,col);
            Lines.AddLine(t2, t3, col);
            Lines.AddLine(t3, t4, col);
            Lines.AddLine(t4, t1, col);

            Lines.AddLine(b1, b2, col);
            Lines.AddLine(b2, b3, col);
            Lines.AddLine(b3, b4, col);
            Lines.AddLine(b4, b1, col);

            Lines.AddLine(t1, b1, col);
            Lines.AddLine(t2, b2, col);
            Lines.AddLine(t3, b3, col);
            Lines.AddLine(t4, b4, col);

            Lines.Finalize();

            foreach(var cnode in Child)
            {
                cnode.AddBBLines(col);
            }

            //Lines.AddLine(bb.Min,bb.Min )


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
                ParticleFX = new Shader._3D.EBasicParticle();
                int bb = 0;
            }
            AlwaysFaceCamera = false;
            Color = new Vector4(1, 1, 1, 1);
            Spin = 0;
            PointMeshes = new List<Mesh.MeshPoints>();
            //XBody = new PXBox(1, 1, 1);

        }
        public float GetRadius()
        {
            float br = -1;
            foreach (var mesh in Meshes)
            {

                foreach (var vertex in mesh.Vertices)
                {

                    //      none = false;
                    Vector3 pos = vertex.Pos;
                    float radius = pos.LengthSquared;
                    if (radius > br) br = radius;

                }

            }
            return br;
        }
        public BoundingBox GetBounds()
        {

            Vector3 min, max;

            min = new Vector3(1200, 1200, 1200);
            max = new Vector3(-1200, -1200, -1200);
            bool none = true;

            foreach(var mesh in Meshes)
            {

                foreach(var vertex in mesh.Vertices)
                {

                    none = false;
                    var pos = vertex.Pos * LocalScale;

                    if (pos.X < min.X) min.X = pos.X;
                    if (pos.Y < min.Y) min.Y = pos.Y;
                    if (pos.Z < min.Z) min.Z = pos.Z;

                    if (pos.X > max.X) max.X = pos.X;
                    if (pos.Y > max.Y) max.Y = pos.Y;
                    if (pos.Z > max.Z) max.Z = pos.Z;

                }

            }

            var res = new BoundingBox();


            if (none)
            {
                min = new Vector3(0, 0, 0);
                max = new Vector3(0, 0, 0);
            }

            res.Min = min;
            res.Max = max;

            //res.Min = res.Min * LocalScale;
           // res.Max = res.Max * LocalScale;

            return res;

        }


        public void Update()
        {
            if (AlwaysFaceCamera)
            {
                LookAt(SceneGlobal.ActiveCamera.LocalPosition, new Vector3(0, 1, 0));
                LocalRotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Spin)) * LocalRotation;



            }
            if (CurrentAnim == null)
            {
                return;
            }
            CurrentAnim.Update();
            Animator.SetTime(CurrentAnim.CurTime);


        }

        public virtual void Rotate(float pitch, float yaw, float roll = 0)
        {
            LocalRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(pitch)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(yaw));// * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(roll));
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
            
            switch (Type)
            {
                case NodeType.Entity:
                    var fx = Global.GlobalEffects.EmissiveFX;
                    SceneGlobal.ActiveNode = this;

                    //var rm = GetModule<Scene.Modules.ModuleMesh3D>() as Scene.Modules.ModuleMesh3D;


                    fx.Bind();

                    Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                    Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                    Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;



                    fx.SetUniform("proj", pm);
                    fx.SetUniform("model", mm);
                    fx.SetUniform("view", vm);
                    fx.SetUniform("tEmissive", 0);

                    foreach (var mesh in Meshes)
                    {
                        if (mesh.Material.EmissiveMap != null)
                        {
                            mesh.Material.EmissiveMap.Bind(Texture.TextureUnit.Unit0);

                            mesh.DrawBindOnly();
                        }

                    }
                    fx.Release();
                    break;
                case NodeType.Actor:

                    var afx = Global.GlobalEffects.EmissiveAnimFX;
                    
                    SceneGlobal.ActiveNode = this;
                    var finalMatrices = Animator.GetFinalBoneMatrices();
                    //var rm = GetModule<Scene.Modules.ModuleMesh3D>() as Scene.Modules.ModuleMesh3D;


                    afx.Bind();
                    

                    Matrix4 apm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                    Matrix4 avm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                    Matrix4 amm = Scene.SceneGlobal.ActiveNode.WorldMatrix;
                    


                    afx.SetUniform("mProj", apm);
                    afx.SetUniform("mModel", amm);
                    afx.SetUniform("mView", avm);
                    afx.SetUniform("tEmissive", 0);

                    for (int i = 0; i < finalMatrices.Count; ++i)
                    {
                        afx.SetUniform("bone_transforms[" + i + "]", finalMatrices[i]);
                    }


                    foreach (var mesh in Meshes)
                    {
                        if (mesh.Material.EmissiveMap != null)
                        {
                            mesh.Material.EmissiveMap.Bind(Texture.TextureUnit.Unit0);

                            mesh.DrawBindOnly();

                            mesh.Material.EmissiveMap.Release(Texture.TextureUnit.Unit0);
                        }

                    }
                    afx.Release();

                    break;
            }



            foreach (var child in Child)
            {
                child.RenderEmissive();
            }
        }
        public void RenderDepth()
        {
            switch (Type)
            {
                case NodeType.Entity:

                    SceneGlobal.ActiveNode = this;
                    var rm = GetModule<Scene.Modules.ModuleMesh3D>() as Scene.Modules.ModuleMesh3D;
                    if (Meshes.Count > 0)
                    {

                        var fx = Global.GlobalEffects.DepthFX;

                        fx.Bind();

                        Matrix4 epm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                        Matrix4 evm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                        Matrix4 emm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

                        fx.SetUniform("proj", epm);
                        fx.SetUniform("model", emm);
                        fx.SetUniform("view", evm);
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
                    break;
                case NodeType.Actor:

                    var finalMatrices = Animator.GetFinalBoneMatrices();
                    SceneGlobal.ActiveNode = this;
                    var afx = Global.GlobalEffects.DepthAnimMeshFX;

                    Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
                    Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
                    Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

                    var light = Scene.SceneGlobal.ActiveLight;

                    foreach (var mesh in Meshes)
                    {

                        //mesh.Material.ColorMap.Bind(Texture.TextureUnit.Unit0);
                        //mesh.Material.NormalMap.Bind(Texture.TextureUnit.Unit1);
                        //mesh.Material.SpecularMap.Bind(Texture.TextureUnit.Unit3);
                        //light.ShadowFB.Cube.Bind(2);

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
                       afx.SetUniform("camP", SceneGlobal.ActiveCamera.LocalPosition);
                        afx.SetUniform("minZ", SceneGlobal.ActiveCamera.MinDepth);
                        afx.SetUniform("maxZ", SceneGlobal.ActiveCamera.MaxDepth);
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
            foreach (var child in Child)
            {
                child.RenderDepth();
            }
        }
        public void Render()
        {

            switch (Type)
            {
                case NodeType.Particle:

                    GLState state = new GLState();

                    state.DepthTest = true;
                    state.DepthMode = DepthFunc.LEqual;
                    state.Blend = true;
                    state.BlendMode = BlendFunc.Additive;
                    state.VX = 0;
                    state.VY = 0;
                    state.VW = App.AppInfo.FrameWidth;
                    state.VH = App.AppInfo.FrameHeight;
                    state.CullFace = false;
                    state.DepthWrite = false;
                    state.Bind();

                    SceneGlobal.ActiveNode = this;

                    ParticleFX.Bind();

                    ParticleFX.SetUniform("proj", SceneGlobal.ActiveCamera.ProjectionMatrix);
                    ParticleFX.SetUniform("model", SceneGlobal.ActiveNode.WorldMatrix);
                    ParticleFX.SetUniform("view", SceneGlobal.ActiveCamera.WorldMatrix);

                    ParticleFX.SetUniform("tCol", 0);

                    foreach (var mesh in PointMeshes)
                    {
                        mesh.Image.Bind(Texture.TextureUnit.Unit0);
                        mesh.Draw();
                        mesh.Image.Release(Texture.TextureUnit.Unit0);
                    }

                    ParticleFX.Release();

                    break;
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

                        mesh.Material.ColorMap.Release(Texture.TextureUnit.Unit0);
                        mesh.Material.NormalMap.Release(Texture.TextureUnit.Unit1);
                        mesh.Material.SpecularMap.Release(Texture.TextureUnit.Unit3);

                    }

                    int a = 0;

                    break;
            }

            if (Lines != null)
            {
                Lines.Draw();
            }

        }

        public void LookAt(Vector3 target,Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, -(target - LocalPosition), up);
            //Console.WriteLine("Local:" + LocalPos.ToString() + " TO:" + p.ToString());
            //m=m.ClearTranslation();

            //   m = m.Inverted();
            //m = m.ClearScale();
            //m = m.ClearProjection();
            m = m.Inverted();

            LocalRotation = m;
        }
        public void LookAtZero(Vector3 p, Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalRotation = m;
        }

    }
}
