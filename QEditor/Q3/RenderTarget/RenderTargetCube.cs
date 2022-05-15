using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Texture;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Q.RenderTarget
{
    public class RenderTargetCube
    {



        public FramebufferHandle FBO;
        public RenderbufferHandle FBD;
        public TextureCube Cube;
        public int W, H;

        public RenderTargetCube(int w, int h)
        {
            W = w;
            H = h;
            Cube = new TextureCube(w, h);
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            FBD = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, FBD);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent32f, w, h);
            // GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            //G/             L.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, FBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX, Cube.Handle, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, FBD);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX, Cube.Handle, 0);

            CheckFBO();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
            Cube.Release(0);
        }

        private static void CheckFBO()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                while (true)
                {
                }
            }
        }

        public TextureTarget SetFace(int face)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            //SetVP.Set(0, 0, W, H);
            GL.Viewport(0, 0, W, H);


            App.AppInfo.FrameWidth = W;
            App.AppInfo.FrameHeight = H;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, (TextureTarget)(((int)TextureTarget.TextureCubeMapPositiveX) + face), Cube.Handle, 0);
            CheckFBO();
            int af = (int)TextureTarget.TextureCubeMapPositiveX + face;

            TextureTarget at = (TextureTarget)(((int)TextureTarget.TextureCubeMapPositiveX) + face);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            return at;
        }

        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle.Zero);
            GL.Viewport(0, 0, App.AppInfo.Width, App.AppInfo.Height);
            App.AppInfo.FrameWidth = App.AppInfo.Width;
            App.AppInfo.FrameHeight = App.AppInfo.Height;
        }
    }

}
