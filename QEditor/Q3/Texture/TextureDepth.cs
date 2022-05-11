using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Q.Texture
{
    public class TextureDepth : Texture
    {

        public TextureDepth(int width, int height) 
        {
            Width = width;
            Height = height;
            Raw = new byte[Width * Height];
            Format = InternalFormat.Rgb;
            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.TextureStorage2D(Handle, 1, SizedInternalFormat.DepthComponent32, Width, Height);
            unsafe
            {
                GCHandle pinnedArray = GCHandle.Alloc(Raw, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();
               // GL.TextureSubImage2D(Handle, 0, 0, 0, Width, Height, PixelFormat.DepthComponent, PixelType.UnsignedByte, pointer);
                //Console.WriteLine("Created texture2D. W:" + Width + " H:" + Height + " Handle:" + Handle.Handle);
            }
            GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
         
            Loading = false;
            DataBound = true;
        }

        public override void Bind(TextureUnit unit)
        {
            base.Bind(unit);
            uint t_unit = (uint)unit;

            GL.BindImageTexture(t_unit, Handle, 0, false, 0, BufferAccessARB.ReadOnly, Format);
        }

        


    }
}
