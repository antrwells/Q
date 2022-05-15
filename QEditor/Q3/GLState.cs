using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Q
{
    public enum DepthFunc
    {
        LEqual,Equal,GEqual
    }

    public enum BlendFunc
    {
        Solid,Additive,Alpha
    }
    public enum CullMode
    {
        Back,Front,None
    }

    public class GLState
    {
    
        public bool DepthTest
        {
            get;
            set;
        }


        public DepthFunc DepthMode
        {
            get;
            set;
        }

        public bool CullFace
        {
            get;
            set;
        }

        public bool Blend
        {
            get;
            set;
        }

        public CullMode CullFunc
        {
            get;
            set;
        }

        public BlendFunc BlendMode
        {
            get;
            set;
        }

        public int VX, VY, VW, VH;

        public void Bind()
        {

            if (DepthTest)
            {
                GL.Enable(EnableCap.DepthTest);
                switch (DepthMode)
                {
                    case DepthFunc.Equal:
                        GL.DepthFunc(DepthFunction.Equal);
                        break;
                    case DepthFunc.LEqual:
                        GL.DepthFunc(DepthFunction.Lequal);
                        break;
                    case DepthFunc.GEqual:
                        GL.DepthFunc(DepthFunction.Gequal);
                        break;
                }
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
            }

            if (CullFace)
            {
                GL.Enable(EnableCap.CullFace);
                switch (CullFunc)
                {
                    case CullMode.None:
                        //GL.CullFace(CullFaceMode.
                            
                        break;
                    case CullMode.Front:
                        GL.CullFace(CullFaceMode.Front);
                        break;
                    case CullMode.Back:
                        GL.CullFace(CullFaceMode.Back);
                        break;
                }
            }
            else
            {
                GL.Disable(EnableCap.CullFace);
            }

            if (Blend)
            {
                GL.Enable(EnableCap.Blend);
                switch (BlendMode)
                {
                    case BlendFunc.Solid:
                        GL.BlendFunc(BlendingFactor.One, BlendingFactor.Zero);
                        break;
                    case BlendFunc.Additive:
                        GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                        break;
                    case BlendFunc.Alpha:
                        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                        break;
                }
            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }

            GL.Viewport(VX, VY, VW, VH);
        }

        public void SetViewport(int x, int y, int w, int h)
        {
            VX = x;
            VY = y;
            VW = w;
            VH = h;
        }

    } 
}
