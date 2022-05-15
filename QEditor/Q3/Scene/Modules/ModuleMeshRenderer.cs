using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Scene;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Q.Mesh;
namespace Q.Scene.Modules
{
    public class ModuleMeshRenderer : NodeModule
    {
        public ModuleMeshRenderer()
        {
            
        }

        public void Create()
        {
            
        }

        
    }
    /*
    public class VertexData<T>
    {
        public int StrideSize = 0;
        public int ComponentSize = 0;
        public int Components = 0;
        public int Vertices = 0;
        public int Indices = 0;
        public T[] Data = null;
        public int[] Index = null;

        public virtual void Init(int vertexCount, int components, int strideSize, int componentSize, int indexCount)
        {
            Data = new T[vertexCount * components];
            StrideSize = strideSize;
            ComponentSize = componentSize;
            Components = components;
            Vertices = vertexCount;
            Indices = indexCount;
            Index = new int[indexCount];
        }
    }
    public class VVBO 
    {
        public int vert_vbo, norm_vbo, uv_vbo, bi_vbo, tan_vbo;
        public int va, vbo, eb, tvbo, nbvo, tanvbo, bivbo;
        public VertexData<float> dat = null;
        public Mesh3D md = null;
        public int Vertices = 0, Indices = 0;
        public VVBO(int vc, int ic) 
        {
            Vertices = vc;
            Indices = ic;
        }

        public void SetData(VertexData<float> d)
        {
            dat = d;
        }

        public void SetMesh(Mesh3D m)
        {
            md = m;
        }

      

        public void Update()
        {
            float[] verts = new float[md.VerticesCount * 3];
            float[] norms = new float[md.VerticesCount * 3];
            float[] bi = new float[md.VerticesCount * 3];
            float[] tan = new float[md.VerticesCount * 3];
            int vi = 0;
            for (int i = 0; i < md.VerticesCount; i++)
            {
                verts[vi] = md.Vertices[i].Pos.X;
                verts[vi + 1] = md.Vertices[i].Pos.Y;
                verts[vi + 2] = md.Vertices[i].Pos.Z;
                norms[vi] = md.Vertices[i].Norm.X;
                norms[vi + 1] = md.Vertices[i].Norm.Y;
                norms[vi + 2] = md.Vertices[i].Norm.Z;
                bi[vi] = md.Vertices[i].BiNorm.X;
                bi[vi + 1] = md.Vertices[i].BiNorm.Y;
                bi[vi + 2] = md.Vertices[i].BiNorm.Z;
                tan[vi] = md.Vertices[i].Tan.X;
                tan[vi + 1] = md.Vertices[i].Tan.Y;
                tan[vi + 2] = md.Vertices[i].Tan.Z;

                vi += 3;
            }

            //vert_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, vert_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), verts, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, norm_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), norms, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bi_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), bi, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, tan_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), tan, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public override void Final()
        {
            va = GL.GenVertexArray();
            GL.BindVertexArray(va);

            float[] verts = new float[md.NumVertices * 3];
            float[] norms = new float[md.NumVertices * 3];
            float[] bi = new float[md.NumVertices * 3];
            float[] tan = new float[md.NumVertices * 3];
            float[] uv = new float[md.NumVertices * 2];
            int vi = 0;
            for (int i = 0; i < md.NumVertices; i++)
            {
                verts[vi] = md.VertexData[i].Pos.X;
                verts[vi + 1] = md.VertexData[i].Pos.Y;
                verts[vi + 2] = md.VertexData[i].Pos.Z;
                norms[vi] = md.VertexData[i].Norm.X;
                norms[vi + 1] = md.VertexData[i].Norm.Y;
                norms[vi + 2] = md.VertexData[i].Norm.Z;
                bi[vi] = md.VertexData[i].BiNorm.X;
                bi[vi + 1] = md.VertexData[i].BiNorm.Y;
                bi[vi + 2] = md.VertexData[i].BiNorm.Z;
                tan[vi] = md.VertexData[i].Tan.X;
                tan[vi + 1] = md.VertexData[i].Tan.Y;
                tan[vi + 2] = md.VertexData[i].Tan.Z;

                vi += 3;
            }
            vi = 0;
            for (int i = 0; i < md.NumVertices; i++)
            {
                uv[vi] = md.VertexData[i].UV.X;
                uv[vi + 1] = md.VertexData[i].UV.Y;
                vi += 2;
            }

            vert_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vert_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), verts, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            uv_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, uv_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 2 * 4), uv, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            norm_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, norm_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), norms, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            bi_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bi_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), bi, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);

            tan_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tan_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), tan, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);

            //GL.EnableVertexAttribArray(1);

         
            GL.BindVertexArray(0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(3);
            GL.DisableVertexAttribArray(4);

            eb = GL.GenBuffer();

            uint[] indices = new uint[md.TriData.Length * 3];

            int ti = 0;
            for (int i = 0; i < md.TriData.Length; i++)
            {
                indices[ti] = (uint)md.TriData[i].V0;
                indices[ti + 1] = (uint)md.TriData[i].V1;
                indices[ti + 2] = (uint)md.TriData[i].v2;
                ti += 3;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eb);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices * 4), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Bind()
        {
            GL.BindVertexArray(va);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eb);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            // GL.EnableVertexAttribArray(3); GL.EnableVertexAttribArray(4);
        }

        public override void Release()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public override void Visualize(int sub)
        {
            GL.DrawElements(BeginMode.Triangles, Indices, DrawElementsType.UnsignedInt, md.Subs[sub].FaceStart * 3);
        }

        public override void Visualize()
        {
            GL.DrawElements(BeginMode.Triangles, Indices, DrawElementsType.UnsignedInt, 0);
        }
    }
    */
}

