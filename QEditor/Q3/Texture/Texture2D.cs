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
    public class Texture2D : Texture
    {
        public static Dictionary<string,Texture> Cached = new Dictionary<string, Texture>();
        public void _LoadDataBM()
        {

            string path = LoadPath;

            Bitmap image_data = new Bitmap(path);

            Width = image_data.Width;
            Height = image_data.Height;
            
  //          Width = image_width;
//            Height = image_height;

            Raw = new byte[Width * Height * 4];

            int raw_os = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {

                    var pix = image_data.GetPixel(x, y);

                    Raw[raw_os++] = (byte)pix.R;
                    Raw[raw_os++] = (byte)pix.G;
                    Raw[raw_os++] = (byte)pix.B;
                    Raw[raw_os++] = (byte)pix.A;


                }
            }

            FileStream fs = new FileStream(path + ".cache", FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);

            w.Write(Width);
            w.Write(Height);
            w.Write(4);

            w.Write(Raw);



            w.Flush();
            fs.Flush();
            w.Close();
            fs.Close();
            Loading = false;

        }
        public void _LoadData()
        {
            

            string path = LoadPath;

            
            
            FileStream fi = new FileStream(path + ".cache", FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fi);

            Width = r.ReadInt32();
            Height = r.ReadInt32();
            int bpp = r.ReadInt32();

            Raw = new byte[Width * Height * 4];

            r.Read(Raw, 0, Width * Height * 4);

            r.Close();
            fi.Close();
            Loading = false;
        }

        public Texture2D(int width, int height)
        {
            Console.WriteLine("Creating tex W/H");
            Width = width;
            Height = height;
           // Raw = new byte[Width * Height * 4];
            Format = InternalFormat.Rgba8;
            Handle = GL.CreateTexture(TextureTarget.Texture2d);
            GL.TextureStorage2D(Handle, 1, SizedInternalFormat.Rgba8, Width, Height);
            unsafe
            {
               // GCHandle pinnedArray = GCHandle.Alloc(Raw, GCHandleType.Pinned);
               // IntPtr pointer = pinnedArray.AddrOfPinnedObject();
               // GL.TextureSubImage2D(Handle, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pointer);
               // pinnedArray.Free();
                //Console.WriteLine("Created texture2D. W:" + Width + " H:" + Height + " Handle:" + Handle.Handle);
            }
            GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            //GL.GenerateTextureMipmap(Handle);
            Loading = false;
            DataBound = true;

        }

        public void CopyTex(int x,int y)
        {
            //Bind(TextureUnit.Unit0);
            //GL.CopyTexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, x, y, Width, Height);
            GL.CopyTextureSubImage2D(Handle, 0, 0, 0, x, y, Width, Height);
            //GL.CopyT
            //Release(TextureUnit.Unit0);
            
        }

        public Texture2D(Q.Pixels.PixelMap map)
        {
            //return;
            Console.WriteLine("Creating texture.");
            Width = map.Width;
            Height = map.Height;
            Format = InternalFormat.Rgba8;
            Raw = map.Data;
            //  BindData();

            GL.Enable(EnableCap.Texture2d);
             Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2d, Handle);
            GL.TexImage2D(TextureTarget.Texture2d, 0, (int)InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Raw);
            Loading = false;
            DataBound = true;

            //Handle = GL.CreateTexture(TextureTarget.Texture2d);
            //GL.TextureStorage2D(Handle, 1, SizedInternalFormat.Rgba8, Width, Height);


            /*
            unsafe
            {
                GCHandle pinnedArray = GCHandle.Alloc(Raw, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                GL.TextureSubImage2D(Handle, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pointer);
                //Console.WriteLine("Created texture2D. W:" + Width + " H:" + Height + " Handle:" + Handle.Handle);
            }
            */
            GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        }
        
        

        public override void DestroyTexture()
        {
            //base.DestroyTexture();
       
         //   {
                GL.DeleteTexture(Handle);
                //GL.delete
               
                Raw = null;
                DestroyNow = false;
                Destroyed = true;
                //Console.WriteLine("Tex destroyed.");
       //     }
        }

        public Texture2D(string path,bool force_alpha = false)
        {
          
            Console.WriteLine("Loading texture.");
            int image_width=64;
            int image_height=64;

            Format = InternalFormat.Rgba8;

            byte[] raw;
            Handle = GL.GenTexture();

            if (File.Exists(path + ".cache"))
            {

                LoadPath = path;

                LoadThread = new Thread(new ThreadStart(_LoadData));
                Loading = true;
                DataBound = false;
                LoadThread.Start();
               


            }
            else
            {

                LoadPath = path;
                LoadThread = new Thread(new ThreadStart(_LoadDataBM));
                Loading = true;
                DataBound = false;
                LoadThread.Start();

               

            }

         
        }


        public void BindData()
        {
            GL.Enable(EnableCap.Texture2d);
           // Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2d, Handle);
            GL.TexImage2D(TextureTarget.Texture2d, 0,(int)InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Raw);

            DataBound = true;
            
            //Handle = GL.CreateTexture(TextureTarget.Texture2d);
            //GL.TextureStorage2D(Handle, 1, SizedInternalFormat.Rgba8, Width, Height);


            /*
            unsafe
            {
                GCHandle pinnedArray = GCHandle.Alloc(Raw, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                GL.TextureSubImage2D(Handle, 0, 0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pointer);
                //Console.WriteLine("Created texture2D. W:" + Width + " H:" + Height + " Handle:" + Handle.Handle);
            }
            */
            GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(Handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
           // GL.GenerateTextureMipmap(Handle);
        }

        public override void Bind(TextureUnit unit)
        {

            if (Loading) return;
            if (DataBound == false)
            {
                DataBound = true;
                BindData();               

            }
            
        
            if (Destroy)
            {
                DestroyAt = Environment.TickCount64 + WaitDestroy;
            }

            uint t_unit = (uint)unit;

            OpenTK.Graphics.OpenGL.TextureUnit texu = OpenTK.Graphics.OpenGL.TextureUnit.Texture0;

            int oid = (int)texu + (int)unit;

            texu = (OpenTK.Graphics.OpenGL.TextureUnit)oid;


            GL.Enable(EnableCap.Texture2d);
            GL.ActiveTexture(texu);
            // GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2d,Handle);


            // GL.BindImageTexture(t_unit, Handle, 0, false, 0, BufferAccessARB.ReadOnly,Format);

            base.Bind(unit);
        }

        public override void Release(TextureUnit unit)
        {
            uint t_unit = (uint)unit;

            OpenTK.Graphics.OpenGL.TextureUnit texu = OpenTK.Graphics.OpenGL.TextureUnit.Texture0;

            int oid = (int)texu + (int)unit;

            texu = (OpenTK.Graphics.OpenGL.TextureUnit)oid;
            base.Release(unit);
            GL.Enable(EnableCap.Texture2d);
            GL.ActiveTexture(texu);
            // GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2d,OpenTK.Graphics.TextureHandle.Zero);
        }

    }
}
