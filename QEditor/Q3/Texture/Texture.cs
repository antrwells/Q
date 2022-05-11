using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Threading;


namespace Q.Texture
{
    public enum TextureUnit
    {
        Unit0 = 0,
        Unit1,
        Unit2,
        Unit3,
        Unit4
      }
    public class Texture
    {

        public TextureHandle Handle
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public byte[] Raw
        {
            get;
            set;
        }

        protected InternalFormat Format
        {
            get;
            set;
        }
        public bool Loading
        {
            get;
            set;
        }

        public bool DataBound
        {
            get;
            set;
        }

        public string LoadPath
        {
            get;
            set;
        }

        public Thread LoadThread
        {
            get;
            set;
        }

        public long DestroyAt
        {
            get;
            set;
        }

        public bool Destroy
        {
            get;
            set;
        }

        public long WaitDestroy
        {
            get;
            set;
        }

        public bool Destroyed
        {
            get;
            set;
        }

        public bool DestroyNow
        {
            get;
            set;
        }

        public static List<Texture> DestroyList = new List<Texture>();

        public void DestoryWhen(long ms)
        {
            DestroyAt = Environment.TickCount64 + ms;
            DestroyList.Add(this);
            WaitDestroy = ms;
            Destroy = true;
            
        }
        
        public static void _DestroyThread()
        {

            //while (true)
            //{

                long time = Environment.TickCount64;
                foreach(var tex in DestroyList.ToArray())
                {
                    if (time > tex.DestroyAt)
                    {
                        //tex.DestroyTexture();
                        tex.DestroyNow = true;
                        tex.DestroyTexture();
                        DestroyList.Remove(tex);
                      //  Console.WriteLine("Destroyed texture!!.");

                    }
                    else
                    {
                   //     Console.WriteLine("Time:" + time + " When:" + tex.DestroyAt);
                    }
                }

                //Console.WriteLine("DCount:" + DestroyList.Count);
              //  System.Threading.Thread.Sleep(10);
            //}
        }

        public static void StartTextureSubSystem()
        {
     
            
        }

        public Texture()
        {
            Loading = false;
        }

        public static List<Texture2D> Threading
        {
            get;
            set;
        }

        public virtual void DestroyTexture()
        {
            
        }

        public virtual void Bind(TextureUnit unit)
        { 
            if(Destroy)
            {
                DestroyAt = Environment.TickCount64 + WaitDestroy;
            }
            
            
           

        }
        
        public virtual void Release(TextureUnit unit)
        {
            
        }

    }
}
