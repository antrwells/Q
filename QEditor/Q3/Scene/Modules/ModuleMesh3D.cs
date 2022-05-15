using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Scene;
using Q.Mesh;
using Q.Shader;
using Q.Shader._3D;
namespace Q.Scene.Modules
{
    public class ModuleMesh3D : NodeModule
    {

        public List<Mesh3D> Meshes
        {
            get;
            set;
        }

        private EBasic3D fx;

        public ModuleMesh3D()
        {
            Meshes = new List<Mesh3D>();
            fx = new EBasic3D();
        }

        public override void RenderModule()
        {
            
            foreach(var mesh in Meshes)
            {
                mesh.Draw(fx);
            }
            //base.RenderModule();
            
        }
    }
}