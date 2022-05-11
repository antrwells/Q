using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

using System;

using Q.App;
using Q.Texture;

namespace Q.RenderTarget
{
  
    public class RenderTarget
    {
        public FramebufferHandle FBO;
        public RenderbufferHandle RB;
        public RenderbufferHandle DB;
   
        public int IW, IH;
        public int DRB = 0;

        ~RenderTarget()
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

        public RenderTarget(int w, int h)
        {
            IW = w;
            IH = h;


            //gen color
            RB = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,InternalFormat.Rgba8, w, h);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,RenderbufferHandle.Zero);

            //gen depth
            DB = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent, w, h);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderbufferHandle.Zero);

            //gen framebuffer
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, RB);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DB);

           var status =  GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete)
            {
                throw new Exception("Framebuffer not complete:" + status.ToString());
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
            Console.WriteLine("CompeteFB");

       
                /*
              * FBO = GL.GenFramebuffer();
             GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
             BB = new Texture2D(w, h);
             DB = new TextureDepth(w, h);
             RB = GL.CreateRenderbuffer();
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
             */
            }

        public void Delete()
        {
            GL.DeleteFramebuffer(FBO);
            GL.DeleteRenderbuffer(RB);
            GL.DeleteRenderbuffer(DB);
        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            // SetVP.Set(0, 0, IW, IH);
            //   AppInfo.RW = IW;
            //  AppInfo.RH = IH;
            AppInfo.FrameWidth = IW;
            AppInfo.FrameHeight = IH;
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
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
            //G//L/.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Release()
        {
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
        }
    }
    
}