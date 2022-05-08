using OpenTK.Graphics.OpenGL;

using System;

using Q.App;
using Q.Texture;

namespace Q.RenderTarget
{
    /*
    public class FrameBufferColor
    {
        public int FBO = 0;
        public Texture2D BB;
        public TextureDepth DB;
        public int IW, IH;
        public int DRB = 0;

        ~FrameBufferColor()
        {
            try
            {
                // GL.DeleteFramebuffer(FBO);
                // GL.DeleteRenderbuffer(DRB);
            }
            catch (Exception e)
            {
            }
        }

        public FrameBufferColor(int w, int h, TextureFormat format = TextureFormat.Normal)
        {
            IW = w;
            IH = h;
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            BB = new Texture2D(w, h, false, format);
            DB = new TextureDepth(w, h);
            DRB = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DRB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, w, h);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DRB);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, BB.ID, 0);
            DrawBuffersEnum db = DrawBuffersEnum.ColorAttachment0;
            GL.DrawBuffers(1, ref db);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                throw (new Exception("Framebuffer failed."));
            }
            Console.WriteLine("Framebuffer success.");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            // SetVP.Set(0, 0, IW, IH);
            AppInfo.RW = IW;
            AppInfo.RH = IH;
            GL.Viewport(0, 0, IW, IH);
            GL.ClearColor(0.2f, 0.2f, 0.2f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void BindFree()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ReleaseFree()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            // SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            AppInfo.RW = AppInfo.W;
            AppInfo.RH = AppInfo.H;
            GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
        }
    }
    */
}