using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Q.Texture;
namespace Q.RenderTarget
{
    public class RenderTargetTex2D
    {
        public int FW, FH;
        FramebufferHandle FB;
        public Texture2D BB;
        public TextureDepth DB;
        RenderbufferHandle RB;

        public RenderTargetTex2D(int w, int h)
        {

            FW = w;
            FH = h;
            FB = GL.CreateFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FB);
            BB = new Texture2D(w, h);
            DB = new TextureDepth(w, h);
            RB = GL.CreateRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,InternalFormat.DepthComponent, w, h);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RB);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, BB.Handle, 0);
            DrawBufferMode db = DrawBufferMode.ColorAttachment0;
            GL.DrawBuffers(1, db);
            var fs = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fs!= FramebufferStatus.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                throw (new Exception("Framebuffer failed:"+fs.ToString()));
            }
            Console.WriteLine("Framebuffer success.");
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderbufferHandle.Zero);

        }

        public void Delete()
        {
            GL.DeleteFramebuffer(FB);

            GL.DeleteRenderbuffer(RB);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FB);
            // SetVP.Set(0, 0, IW, IH);
            App.AppInfo.FrameWidth = FW;
            App.AppInfo.FrameHeight = FH;
            
            GL.Viewport(0, 0,FW,FH);
            GL.ClearColor(0,0,0,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,FramebufferHandle.Zero);
            // SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            App.AppInfo.FrameWidth = App.AppInfo.Width;
            App.AppInfo.FrameHeight = App.AppInfo.Height;
            GL.Viewport(0, 0,App.AppInfo.Width,App.AppInfo.Height);
        }

    }
}
