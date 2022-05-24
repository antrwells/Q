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
    public class VertexPoint
    {
        public Vector3 Point;
        public Vector4 Color;
     
    }
    public class MeshPoints
    {

        public List<VertexPoint> Points = new List<VertexPoint>();

        public Q.Texture.Texture2D Image = null;

        public void Add(VertexPoint point)
        {
            Points.Add(point);
        }

        BufferHandle posBuf, colBuf;
        BufferHandle eBuf;
        VertexArrayHandle vao;

        public void Finalize()
        {

            float[] pos = new float[Points.Count * 3];
            float[] color = new float[Points.Count * 4];

            int pc, cc;

            pc = cc = 0;
            foreach(var point in Points)
            {

                pos[pc++] = point.Point.X;
                pos[pc++] = point.Point.Y;
                pos[pc++] = point.Point.Z;

                color[cc++] = point.Color.X;
                color[cc++] = point.Color.Y;
                color[cc++] = point.Color.Z;
                color[cc++] = point.Color.W;

            }


            posBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, pos, BufferUsageARB.StaticDraw);

            colBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, color, BufferUsageARB.StaticDraw);

            vao = GL.GenVertexArray();

            GL.BindVertexArray(vao);
            
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,0, 0);

            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);



            


        }

        public void Delete()
        {

            GL.DeleteBuffer(posBuf);
            GL.DeleteBuffer(colBuf);
            GL.DeleteVertexArray(vao);

        }

        public void Draw()
        {

            GL.BindVertexArray(vao);
           // GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
           
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            //GL.draw;
            GL.DrawArrays(PrimitiveType.Points, 0, Points.Count);

        }

    }
}
