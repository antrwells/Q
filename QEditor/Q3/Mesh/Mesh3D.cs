using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Q.Shader;
namespace Q.Mesh
{
    public struct Tri
    {
        public int V0, V1, V2;
        public Tri(int v0,int v1,int v2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
        }
    }

    
    public struct Vertex
    {
        public Vector3 Pos;
        public Vector3 Col;
        public Vector3 UV;
        public Vector3 Norm;
        public Vector3 BiNorm;
        public Vector3 Tan;
        public int[] m_BoneIDS = new int[4];
        public float[] m_Weights = new float[4];
        public Vertex()
        {
            Pos = new Vector3(0, 0, 0);
            Col = Pos;
            UV = Pos;
            Norm = Pos;
            BiNorm = Pos;
            Tan = Pos;
            for (int i = 0; i < 4; i++)
            {
                m_BoneIDS[i] = -1;
                m_Weights[i] = 0;
            }

        }
    }

    public class Mesh3D
    {
        public List<Vertex> Vertices
        {
            get;
            set;
        }

        public List<Tri> Tris
        {
            get;
            set;
        }

        public Material.Material3D Material
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int VerticesCount
        {
            get
            {
                return Vertices.Count;
            }
        }

        public Mesh3D()
        {
            Vertices = new List<Vertex>();
            Tris = new List<Tri>();
            Material = null;
        }

        public void SetBoneData(int index,int boneID,float weight)
        {
            int MAX_BONE_WEIGHTS = 4;

            var vertex = Vertices[index];
            for (int i = 0; i < MAX_BONE_WEIGHTS; ++i)
            {
                if (vertex.m_BoneIDS[i]<0)
                {
                
                    vertex.m_BoneIDS[i] = boneID;
                    vertex.m_Weights[i] = weight;
                    Vertices[index] = vertex;
                    break;
                }
            }
            Vertices[index] = vertex;

        }
        public void SetVertex(int id,Vector3 pos,Vector3 tan,Vector3 bi,Vector3 norm,Vector3 uv)
        {
            Vertex vertex = new Vertex();
            vertex.Pos = pos;
            vertex.Tan = tan;
            vertex.BiNorm = bi;
            vertex.Norm = norm;
            vertex.UV = uv;
            Vertices[id] = vertex;
        }

        public void SetTri(int id,int v0,int v1,int v2)
        {
            Tris[id] = new Tri(v0, v1, v2);
        }

        public void AddVertex(Vertex v)
        {
            Vertices.Add(v);
        }

        public void AddTri(Tri t)
        {
            Tris.Add(t);
        }

        public void AddTri(int v0,int v1,int v2)
        {
            Tris.Add(new Tri(v0, v1, v2));

        }


        BufferHandle[] buffer = new BufferHandle[1];
        VertexArrayHandle[] arrays = new VertexArrayHandle[1];
        BufferHandle[] tb = new BufferHandle[1];

        BufferHandle posBuf, uvBuf, normBuf, tanBuf, biBuf;
        BufferHandle boneBuf, weightBuf;

        public void Delete()
        {

            GL.DeleteBuffer(posBuf);
            GL.DeleteBuffer(uvBuf);
            GL.DeleteBuffer(normBuf);
            GL.DeleteBuffer(tanBuf);
            GL.DeleteBuffer(biBuf);
            GL.DeleteBuffer(tb[0]);
            GL.DeleteVertexArray(arrays[0]);

        }
        public void Finalize()
        {

            float[] verts = new float[VerticesCount * 3];
            float[] norms = new float[VerticesCount * 3];
            float[] bi = new float[VerticesCount * 3];
            float[] tan = new float[VerticesCount * 3];
            float[] uv = new float[VerticesCount * 3];

            float[] fdat = new float[VerticesCount*3];
            int[] bones = new int[VerticesCount * 4];
            float[] weights = new float[VerticesCount * 4];
            int vi = 0;
            for (int i = 0; i < VerticesCount; i++)
            {
                verts[vi] = Vertices[i].Pos.X;
                verts[vi+1] = Vertices[i].Pos.Y;
                verts[vi+2] = Vertices[i].Pos.Z;
                uv[vi] = Vertices[i].UV.X;
                uv[vi+1] = Vertices[i].UV.Y;
                uv[vi + 2] = Vertices[i].UV.Z;
                norms[vi] = Vertices[i].Norm.X;
                norms[vi + 1] = Vertices[i].Norm.Y;
                norms[vi + 2] = Vertices[i].Norm.Z;
                bi[vi] = Vertices[i].BiNorm.X;
                bi[vi + 1] = Vertices[i].BiNorm.Y;
                bi[vi + 2] = Vertices[i].BiNorm.Z;
                tan[vi] = Vertices[i].Tan.X;
                tan[vi + 1] = Vertices[i].Tan.Y;
                tan[vi + 2] = Vertices[i].Tan.Z;
         


                vi = vi + 3;
            }
            vi = 0;
            for(int i = 0; i < VerticesCount;i++)
            {
                bones[vi] = Vertices[i].m_BoneIDS[0];
                bones[vi + 1] = Vertices[i].m_BoneIDS[1];
                bones[vi + 2] = Vertices[i].m_BoneIDS[2];
                bones[vi + 3] = Vertices[i].m_BoneIDS[3];
                weights[vi] = Vertices[i].m_Weights[0];
                weights[vi + 1] = Vertices[i].m_Weights[1];
                weights[vi + 2] = Vertices[i].m_Weights[2];
              
                weights[vi + 3] = Vertices[i].m_Weights[3];
                vi = vi + 4;
            }
            posBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer,posBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer,verts, BufferUsageARB.StaticDraw);

            uvBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, uv, BufferUsageARB.StaticDraw);

            normBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, normBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, norms, BufferUsageARB.StaticDraw);

            biBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, biBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, bi, BufferUsageARB.StaticDraw);

            tanBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, tanBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, tan, BufferUsageARB.StaticDraw);

            boneBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, boneBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, bones, BufferUsageARB.StaticDraw);

            weightBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, weightBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, weights, BufferUsageARB.StaticDraw);
            





            GL.GenVertexArrays(arrays);
            GL.BindVertexArray(arrays[0]);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuf);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, normBuf);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, biBuf);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, tanBuf);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, boneBuf);
            GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Int, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, weightBuf);
            GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
            GL.EnableVertexAttribArray(4);
            GL.EnableVertexAttribArray(5);
            GL.EnableVertexAttribArray(6);
            




            /*
             * 

            GL.CreateVertexArrays(arrays);
            GL.VertexArrayVertexBuffer(arrays[0], 0, buffer[0], IntPtr.Zero, (5*3)*4);
            GL.VertexArrayVertexBuffer(arrays[0], 1, buffer[0],new IntPtr(3*4), (5 * 3) * 4);
            GL.VertexArrayVertexBuffer(arrays[0], 2, buffer[0],new IntPtr(3*4*2), (5 * 3) * 4);
            GL.VertexArrayVertexBuffer(arrays[0], 3, buffer[0], new IntPtr(3*4*3), (5 * 3) * 4);
            GL.VertexArrayVertexBuffer(arrays[0], 4, buffer[0], new IntPtr(3 * 4 * 4), (5 * 3) * 4) ;


            //Console.WriteLine("BufferID:" + buffer[0].Handle);
            //Console.WriteLine("ArrayID:" + arrays[0].Handle);

            GL.VertexArrayAttribBinding(arrays[0], 0, 0);
            GL.VertexArrayAttribFormat(arrays[0], 0, 3, VertexAttribType.Float, false, 3 * 4);
           
            GL.EnableVertexArrayAttrib(arrays[0], 0);


            GL.VertexArrayAttribBinding(arrays[0], 1, 1);
            GL.VertexArrayAttribFormat(arrays[0], 1, 3, VertexAttribType.Float, false, 3 * 4);
         
            GL.EnableVertexArrayAttrib(arrays[0], 1);

            GL.VertexArrayAttribBinding(arrays[0], 2, 2);
            GL.VertexArrayAttribFormat(arrays[0], 2, 3, VertexAttribType.Float, false, 3 * 4);
        
            GL.EnableVertexArrayAttrib(arrays[0], 2);


            GL.VertexArrayAttribBinding(arrays[0], 3, 3);
            GL.VertexArrayAttribFormat(arrays[0], 3, 3, VertexAttribType.Float, false, 3 * 4);
       

            GL.EnableVertexArrayAttrib(arrays[0], 3);


            GL.VertexArrayAttribBinding(arrays[0], 4, 4);
            GL.VertexArrayAttribFormat(arrays[0], 4, 3, VertexAttribType.Float, false, 3 * 4);
          
            GL.EnableVertexArrayAttrib(arrays[0], 4);

    */

            ;
            int a = 5;

            uint[] tri = new uint[Tris.Count * 3];
            int ti = 0;
            for (int t = 0; t < Tris.Count; t++)
            {
                tri[ti++] = (uint)Tris[t].V0;
                tri[ti++] = (uint)Tris[t].V1;
                tri[ti++] = (uint)Tris[t].V2;
            }

            tb[0] = GL.CreateBuffer();
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb[0]);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, tri, BufferUsageARB.StaticDraw);
            


        }

        public void DrawBindOnly()
        {
            GL.BindVertexArray(arrays[0]);
          //  GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);


            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            //GL.draw;
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb[0]);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)

            GL.DrawElements(PrimitiveType.Triangles, Tris.Count * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

        }


        public void DrawParticle(Effect fx, Vector4 color)
        {
            Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
            Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
            Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

            var light = Scene.SceneGlobal.ActiveLight;

            fx.Bind();
            Material.ColorMap.Bind(Texture.TextureUnit.Unit0);
            Material.NormalMap.Bind(Texture.TextureUnit.Unit1);
            Scene.SceneGlobal.ActiveLight.ShadowFB.Cube.Bind(2);
            fx.SetUniform("viewPos", Scene.SceneGlobal.ActiveCamera.LocalPosition);
            fx.SetUniform("tCol", 0);
            fx.SetUniform("tNorm", 1);
            fx.SetUniform("tSpec", 3);
            fx.SetUniform("tShadow", 2);
            fx.SetUniform("tEnv", 4);
            fx.SetUniform("lightDepth", Scene.SceneGlobal.ActiveCamera.MaxDepth);
            fx.SetUniform("lPos", light.LocalPosition);
            fx.SetUniform("lDiff", light.Diffuse);
            fx.SetUniform("lSpec", light.Specular);
            fx.SetUniform("lRange", light.Range);
            fx.SetUniform("shadowMapping", 1);
            fx.SetUniform("envMapping", 0);
            fx.SetUniform("refract", 0);
            fx.SetUniform("pColor", color);


            fx.SetUniform("tColor", 0);
            fx.SetUniform("proj", pm);
            fx.SetUniform("model", mm);
            fx.SetUniform("view", vm);


            // fx.SetUniform("texSize", new Vector2(Material.ColorMap.Width,Material.ColorMap.Height));
            //fx.SetUniform("drawCol", new);

            GL.BindVertexArray(arrays[0]);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            //GL.draw;
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb[0]);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)

            GL.DrawElements(PrimitiveType.Triangles, Tris.Count * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);


            fx.Release();

            Scene.SceneGlobal.ActiveLight.ShadowFB.Cube.Release(2);
        }
        public void Draw(Effect fx)
        {
            //Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0, 1.0f);

            Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
            Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
            Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

            var light = Scene.SceneGlobal.ActiveLight;

            fx.Bind();
            Material.ColorMap.Bind(Texture.TextureUnit.Unit0);
            Material.NormalMap.Bind(Texture.TextureUnit.Unit1);
            Scene.SceneGlobal.ActiveLight.ShadowFB.Cube.Bind(2);
            fx.SetUniform("viewPos", Scene.SceneGlobal.ActiveCamera.LocalPosition);
            fx.SetUniform("tCol",0);
            fx.SetUniform("tNorm", 1);
            fx.SetUniform("tSpec", 3);
            fx.SetUniform("tShadow", 2);
            fx.SetUniform("tEnv", 4);
            fx.SetUniform("lightDepth", Scene.SceneGlobal.ActiveCamera.MaxDepth);
            fx.SetUniform("lPos", light.LocalPosition);
            fx.SetUniform("lDiff", light.Diffuse);
            fx.SetUniform("lSpec", light.Specular);
            fx.SetUniform("lRange", light.Range);
            fx.SetUniform("shadowMapping", 1);
            fx.SetUniform("envMapping", 0);
            fx.SetUniform("refract", 0);
            


            fx.SetUniform("tColor", 0);
            fx.SetUniform("proj", pm);
            fx.SetUniform("model",mm);
            fx.SetUniform("view",vm);
        
            
           // fx.SetUniform("texSize", new Vector2(Material.ColorMap.Width,Material.ColorMap.Height));
            //fx.SetUniform("drawCol", new);

            GL.BindVertexArray(arrays[0]);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            //GL.draw;
             GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb[0]);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
           // GL.DrawElements(PrimitiveType.Triangles,)

            GL.DrawElements(PrimitiveType.Triangles, Tris.Count*3, DrawElementsType.UnsignedInt, IntPtr.Zero);
            

            fx.Release();

            Scene.SceneGlobal.ActiveLight.ShadowFB.Cube.Release(2);
        }


        public Mesh3D Clone()
        {

            var res = new Mesh3D();

            res.tb = tb;
            res.buffer = buffer;
            res.arrays = arrays;
            res.biBuf = biBuf;
            res.boneBuf = boneBuf;
            res.buffer = buffer;
            res.normBuf = normBuf;
            res.posBuf = posBuf;
            res.tanBuf = tanBuf;
            res.uvBuf = uvBuf;
            res.Vertices = Vertices;
            res.Tris = Tris;

            res.Material = Material;

            return res;

        }
            

    }
}
