using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
namespace Q.Draw.Simple
{
    public delegate void SetFXPars();
    public enum Blend
    {
        None,Additive,Mod,Alpha
    }
    public class Draw2D
    {

        private float[] pos;
        private float[] uv;
        private float[] col;
        private BufferHandle posBuf, uvBuf, colBuf;
        private BufferHandle indexBuf;
        private VertexArrayHandle vao;
        Q.Shader._2D.EXBasic2D fx1;
        Q.Shader._2D.EXBasicBlur fxblur;
        Blend BlendMode = Blend.None;

        public GLState DrawState;

        public Draw2D()
        {

            pos = new float[4 * 2];
            uv = new float[4 * 2];
            col = new float[4 * 4];
            fx1 = new Shader._2D.EXBasic2D();
            fxblur = new Shader._2D.EXBasicBlur();
            int a = 1;
            BlendMode = Blend.Additive;
            DrawState = new GLState();
            DrawState.Blend = true;
            DrawState.BlendMode = BlendFunc.Solid;
            DrawState.DepthTest = false;
            DrawState.CullFace = false;
       

        }

        public void SetBlend(Blend blendmode)
        {
            BlendMode = blendmode;
        }
        
        public void GenGL()
        {

            posBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, pos, BufferUsageARB.StaticDraw);

            uvBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, uv, BufferUsageARB.StaticDraw);

            colBuf = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.BufferData(BufferTargetARB.ArrayBuffer, col, BufferUsageARB.StaticDraw);

            vao = GL.GenVertexArray();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, posBuf);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuf);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, colBuf);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 0);


            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            uint[] tris = new uint[6];
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;
            tris[3] = 2;
            tris[4] = 3;
            tris[5] = 0;

            indexBuf = GL.CreateBuffer();
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuf);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, tris, BufferUsageARB.StaticDraw);


        }


        void Gen(int x, int y, int w, int h, Vector4 c)
        {
            pos[0] = x;
            pos[1] = y;

            pos[2] = x + w;
            pos[3] = y;
            pos[4] = x + w;
            pos[5] = y + h;
            pos[6] = x;
            pos[7] = y + h;

            uv[0] = 0;
            uv[1] = 0;

            uv[2] = 1;
            uv[3] = 0;

            uv[4] = 1;
            uv[5] = 1;
            uv[6] = 0;
            uv[7] = 1;


            col[0] = c.X;
            col[1] = c.Y;
            col[2] = c.Z;
            col[3] = c.W;


            col[4] = c.X;
            col[5] = c.Y;
            col[6] = c.Z;
            col[7] = c.W;

            col[8] = c.X;
            col[9] = c.Y;
            col[10] = c.Z;
            col[11] = c.W;

            col[12] = c.X;
            col[13] = c.Y;
            col[14] = c.Z;
            col[15] = c.W;

            GenGL();
        }

        public void RectBlur(int x, int y, int w, int h, Q.Texture.Texture2D tex, Vector4 c, float blur)
        {
            
            Gen(x, y, w, h, c);
            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, -1.0f, 1.0f);
            
            DrawState.VX = 0;
            DrawState.VY = 0;
            DrawState.VW = App.AppInfo.FrameWidth;
            DrawState.VH = App.AppInfo.FrameHeight;
            DrawState.Bind();
        
            //   GL.Disable(EnableCap.DepthTest);
            //GL.Disable(EnableCap.CullFace);
      
            fxblur.Bind();
            tex.Bind(Texture.TextureUnit.Unit0);
            fxblur.SetUniform("tR", 0);
            fxblur.SetUniform("proj", pm);
            fxblur.SetUniform("blur", blur);
            fxblur.SetUniform("res", new Vector2(App.AppInfo.FrameWidth/2, App.AppInfo.FrameHeight/2));
            //    fx1.SetUniform("texSize", new Vector2(tex.Width, tex.Height));
            //      fx1.SetUniform("drawCol", col);e
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuf);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)



            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            fxblur.Release();

    

            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(posBuf);
            GL.DeleteBuffer(uvBuf);
            GL.DeleteBuffer(colBuf);
            GL.DeleteBuffer(indexBuf);

            //   GL.Disable(EnableCap.Blend);

        }
        public void RectFX(Q.Shader.Effect fx, int x, int y, int w, int h, Q.Texture.Texture2D tex, Vector4 c, SetFXPars pars)
        {
            Gen(x, y, w, h, c);

            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight, 0, -1.0f, 1.0f);

            switch (BlendMode)
            {
                case Blend.None:
                    GL.Disable(EnableCap.Blend);
                    break;
                case Blend.Additive:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);

                    break;
                case Blend.Alpha:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                    break;
                    //GL.Disable(EnableCap.Blend);

            }

            //   GL.Disable(EnableCap.DepthTest);
            //GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            fx.Bind();
            tex.Bind(Texture.TextureUnit.Unit0);
            fx.SetUniform("tR", 0);
            fx.SetUniform("proj", pm);
            pars.Invoke();
            //    fx1.SetUniform("texSize", new Vector2(tex.Width, tex.Height));
            //      fx1.SetUniform("drawCol", col);e
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuf);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)



            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            fx.Release();
            GL.Disable(EnableCap.Blend);

            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(posBuf);
            GL.DeleteBuffer(uvBuf);
            GL.DeleteBuffer(colBuf);
            GL.DeleteBuffer(indexBuf);

            //  GL.Enable(EnableCap.Depth
        }
        public void Rect(int x,int y,int w,int h,Q.Texture.Texture2D tex,Vector4 c)
        {
            Gen(x, y, w, h, c);



            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, App.AppInfo.FrameWidth, App.AppInfo.FrameHeight,0, -1.0f, 1.0f);

            DrawState.VX = 0;
            DrawState.VY = 0;
            DrawState.VW = App.AppInfo.FrameWidth;
            DrawState.VH = App.AppInfo.FrameHeight;
            DrawState.Blend = true;
            DrawState.BlendMode = BlendFunc.Alpha;
            DrawState.Bind();

            //   GL.Disable(EnableCap.DepthTest);
           // GL.Disable(EnableCap.CullFace);
          //  GL.Disable(EnableCap.DepthTest);

            fx1.Bind();
            tex.Bind(Texture.TextureUnit.Unit0);
            fx1.SetUniform("tR", 0);
            fx1.SetUniform("proj", pm);
            //    fx1.SetUniform("texSize", new Vector2(tex.Width, tex.Height));
            //      fx1.SetUniform("drawCol", col);e
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuf);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 32);
            // GL.DrawElements(PrimitiveType.Triangles,)



            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            fx1.Release();
        //    GL.Disable(EnableCap.Blend);

            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(posBuf);
            GL.DeleteBuffer(uvBuf);
            GL.DeleteBuffer(colBuf);
            GL.DeleteBuffer(indexBuf);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer,BufferHandle.Zero);
            GL.BindVertexArray(VertexArrayHandle.Zero);
            

            //GL.BindTexture(TextureTarget.Texture2D, 0);


            //      GL.Enable(EnableCap.CullFace);

         //   GL.Enable(EnableCap.DepthTest);




        }

    }
}
