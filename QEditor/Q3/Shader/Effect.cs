using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace Q.Shader
{
    public class Effect
    {
        
        protected ProgramHandle ProgramHandle
        {
            get;
            set;
        }

        protected ShaderHandle VertexHandle
        {
            get;
            set;
        }
        
        protected ShaderHandle FragHandle
        {
            get;
            set;
        }

        protected ShaderHandle GeoHandle
        {
            get;
            set;
        }
        public Effect(string geo,string vert,string frag)
        {
            ProgramHandle = GL.CreateProgram();
            GL.ObjectLabel(ObjectIdentifier.Program, (uint)ProgramHandle.Handle, -1, "Effect:" + vert);
            AttachShader(ShaderType.GeometryShader, geo);
            AttachShader(ShaderType.VertexShader, vert);
            AttachShader(ShaderType.FragmentShader, frag);
            GL.LinkProgram(ProgramHandle);
            int pars = 255;
            unsafe
            {
                int* parp = &pars;

                GL.GetProgramiv(ProgramHandle, ProgramPropertyARB.LinkStatus, parp);
            }
            GL.GetProgramInfoLog(ProgramHandle, out string info);
            Console.WriteLine("ProgramStatus:" + info);
            Console.WriteLine("Link:" + pars);
        }
        public Effect(string vertex_path,string frag_path)
        {
            ProgramHandle = GL.CreateProgram();
            GL.ObjectLabel(ObjectIdentifier.Program, (uint)ProgramHandle.Handle,-1,"Effect:"+vertex_path);

            AttachShader(ShaderType.VertexShader, vertex_path);
            AttachShader(ShaderType.FragmentShader, frag_path);
            GL.LinkProgram(ProgramHandle);
            int pars = 255;
            unsafe
            {
                int* parp = &pars;

                GL.GetProgramiv(ProgramHandle, ProgramPropertyARB.LinkStatus, parp);
            }
            GL.GetProgramInfoLog(ProgramHandle, out string info);
            Console.WriteLine("ProgramStatus:" + info);
            Console.WriteLine("Link:" + pars);

        }
        
        ShaderHandle AttachShader(ShaderType type,string path)
        {
            string text = File.ReadAllText(path);
            ShaderHandle handle = GL.CreateShader(type);
            GL.ShaderSource(handle, text);
            GL.CompileShader(handle);
            GL.AttachShader(ProgramHandle, handle);
            GL.DeleteShader(handle);
            return handle;
            
        }

        public void Bind()
        {
            GL.UseProgram(ProgramHandle);
            BindPars();
        }

        public void Release()
        {

            GL.UseProgram(ProgramHandle.Zero);
            
        }

        public virtual void BindPars()
        {


        }

        public void SetUniform(int loc,int val)
        {
            GL.Uniform1i(loc, val);
        }

        public void SetUniform(string name, int val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.Uniform1i(loc, val);
        }
        public void SetUniform(string name,Vector2 val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.Uniform2f(loc, val.X,val.Y);
        }

        public void SetUniform(string name, Vector3 val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.Uniform3f(loc, val.X, val.Y, val.Z);
        }

        public void SetUniform(string name, Vector4 val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.Uniform4f(loc, val.X, val.Y, val.Z, val.W);
        }

        public void SetUniform(string name, Matrix4 val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.UniformMatrix4f(loc, false,in val);
        }

        public void SetUniform(string name, float val)
        {
            int loc = GL.GetUniformLocation(ProgramHandle, name);
            GL.Uniform1f(loc, val);
        }

    }
}
