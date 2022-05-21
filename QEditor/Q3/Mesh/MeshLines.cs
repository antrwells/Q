using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Q.Mesh
{
    public class LineVertex
    {
        public Vector3 Pos
        {
            get;
            set;
        }

        public Vector4 Color
        {
            get;
            set;
        }

    }
    public class MeshLines
    {

        public List<LineVertex> Vertices
        {
            get;
            set;
        }

        private VertexArrayHandle va;
        private BufferHandle posBuf;
        private BufferHandle colBuf;
        private BufferHandle tb;

        public MeshLines()
        {
            Vertices = new List<LineVertex>();
        }

        public void Add(LineVertex vertex)
        {
            Vertices.Add(vertex);
        }

        public void AddLine(Vector3 p1,Vector3 p2,Vector4 col)
        {
            LineVertex v1 = new LineVertex(), v2= new LineVertex();
            
            v1.Pos = p1;
            v1.Color = col;
            v2.Pos = p2;
            v2.Color = col;

            Vertices.Add(v1);
            Vertices.Add(v2);


        }

        public void Finalize()
        {

            float[] verts = new float[Vertices.Count * 3];
            float[] cols = new float[Vertices.Count * 4];

            int vi = 0;
            int ci = 0;

            foreach(var v in Vertices)
            {
                verts[vi] = v.Pos.X;
                verts[vi + 1] = v.Pos.Y;
                verts[vi + 2] = v.Pos.Z;
                cols[ci] = v.Color.X;
                cols[ci + 1] = v.Color.Y;
                cols[ci + 2] = v.Color.Z;
                cols[ci + 3] = v.Color.W;
                vi += 3;
                ci += 4;
            }
            
            posBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, verts, BufferUsageARB.StaticDraw);

        

            colBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, cols, BufferUsageARB.StaticDraw);

            va = GL.GenVertexArray();
            GL.BindVertexArray(va);

            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);


            int a = 5;

            uint[] tri = new uint[Vertices.Count];
            int ti = 0;
            for (int t = 0; t < Vertices.Count-1; t++)
            {
                tri[t] = (uint)t;
                tri[t + 1] = (uint)t + 1;
            }
        
            tb = GL.CreateBuffer();
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, tri, BufferUsageARB.StaticDraw);

        }

        public void Draw()
        {

            var fx = Global.GlobalEffects.MeshLinesFX;

            Matrix4 pm = Scene.SceneGlobal.ActiveCamera.ProjectionMatrix; //Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), App.AppInfo.Width / App.AppInfo.Height, 0.01f, 1000);  //.CreatePerspectiveOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0.1f, 2500.0f);
            Matrix4 vm = Scene.SceneGlobal.ActiveCamera.WorldMatrix;
            Matrix4 mm = Scene.SceneGlobal.ActiveNode.WorldMatrix;

            fx.Bind();

            fx.SetUniform("proj", pm);
            fx.SetUniform("view", vm);
            fx.SetUniform("model", mm);

            GL.BindVertexArray(va);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            //GL.draw;
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, tb);
            //GL.DrawArrays(PrimitiveType.Lines,0, Vertices.Count*3);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)

          GL.DrawElements(PrimitiveType.Lines, Vertices.Count , DrawElementsType.UnsignedInt, IntPtr.Zero);

            fx.Release();

        }

    }
}
