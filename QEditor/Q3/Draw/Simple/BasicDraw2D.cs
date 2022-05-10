using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using Q.Shader._2D;

/// <summary>
/// The Draw.Simple namespace is a set of easy to use functions to display basic 2D imagary.
/// </summary>
namespace Q.Draw.Simple
{
    public class BasicDraw2D
    {
        BufferHandle[]? buffer;
        VertexArrayHandle[]? arrays;

        float[] data = new float[24];
        EXBasic2D fx1;

        public BasicDraw2D()
        {

            float s = 1.0f;

            SetData(0, 0, 50, 50);

            fx1 = new EXBasic2D();

            GenerateGL();

        }

        private void GenerateGL()
        {
            buffer = new BufferHandle[1];
            arrays = new VertexArrayHandle[1];
            int size = 8 * 4;



            GL.CreateBuffers(buffer);
            GL.NamedBufferStorage<float>(buffer[0], data, BufferStorageMask.DynamicStorageBit);

            GL.CreateVertexArrays(arrays);
            GL.VertexArrayVertexBuffer(arrays[0], 0, buffer[0], IntPtr.Zero, 4 * 4);
            GL.VertexArrayVertexBuffer(arrays[0], 1, buffer[0], new IntPtr(4 * 2), 4 * 4);

            //Console.WriteLine("BufferID:" + buffer[0].Handle);
            //Console.WriteLine("ArrayID:" + arrays[0].Handle);

            GL.EnableVertexArrayAttrib(arrays[0], 0);
            GL.VertexArrayAttribFormat(arrays[0], 0, 2, VertexAttribType.Float, false, 0);
            GL.VertexArrayAttribBinding(arrays[0], 0, 0);

            GL.EnableVertexArrayAttrib(arrays[0], 1);
            GL.VertexArrayAttribFormat(arrays[0], 1, 2, VertexAttribType.Float, false, 0);
            GL.VertexArrayAttribBinding(arrays[0], 1, 1);
        }

        private void SetData(int x, int y, int w, int h)
        {
            data[0] = x;
            data[1] = y;
            data[2] = 0;
            data[3] = 0;
            data[4] = x + w;
            data[5] = y; ;
            data[6] = 1;
            data[7] = 0;
            data[8] = x + w;
            data[9] = y + h;
            data[10] = 1;
            data[11] = 1;
            data[12] = x + w;
            data[13] = y + h;
            data[14] = 1;
            data[15] = 1;
            data[16] = x;
            data[17] = y + h;
            data[18] = 0;
            data[19] = 1;
            data[20] = x;
            data[21] = y;
            data[22] = 0;
            data[23] = 0;
        }

        /// <summary>
        /// This method allows you to draw a image rect, using your own Effect class, to achieve custom effects.
        /// </summary>
        /// <param name="fx">The effect class</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="tex">The texture to use as the imagary.</param>
        /// <param name="col">The color. components should range from 0(dark) to 1(white)</param>
        public void Rect(Q.Shader.Effect fx,int x, int y, int w, int h, Q.Texture.Texture2D tex, Vector4 col)
        {
            //tex.Bind(Texture.TextureUnit.Unit0);

            SetData(x, y, w, h);
            GenerateGL();




            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, 0, 1.0f);

            fx.Bind();
            tex.Bind(Texture.TextureUnit.Unit0);
            fx.SetUniform("image", 0);
            fx.SetUniform("proj", pm);
            fx.SetUniform("texSize", new Vector2(tex.Width, tex.Height));
            fx.SetUniform("drawCol", col);

            GL.BindVertexArray(arrays[0]);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 16);

            fx1.Release();

            GL.DeleteVertexArray(arrays[0]);
            GL.DeleteBuffer(buffer[0]);
            // tex.Release(Texture.TextureUnit.Unit0);

        }

        /// <summary>
        /// A simple method to display an image of any size/color. There is no need for a custom effect, it uses a built-in one.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="tex"></param>
        /// <param name="col"></param>
        public void Rect(int x, int y, int w, int h, Q.Texture.Texture2D tex, Vector4 col)
        {
            //tex.Bind(Texture.TextureUnit.Unit0);

            SetData(x, y, w, h);
            GenerateGL();

           


            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0,App.AppInfo.FrameWidth, App.AppInfo.FrameHeight,0, 0, 1.0f);

            fx1.Bind();
            tex.Bind(Texture.TextureUnit.Unit0);
            fx1.SetUniform("image",0);
            fx1.SetUniform("proj", pm);
            fx1.SetUniform("texSize", new Vector2(tex.Width, tex.Height));
            fx1.SetUniform("drawCol", col);

            GL.BindVertexArray(arrays[0]);
            GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 16);

            fx1.Release();

            GL.DeleteVertexArray(arrays[0]);
            GL.DeleteBuffer(buffer[0]);
           // tex.Release(Texture.TextureUnit.Unit0);

        }

    }
}
