using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Harmony.Shapes
{
    
    public class ShapeTriMesh : CollisionShape
    {

        public List<Mesh.Vertex> Vertices
        {
            get;
            set;
        }

        public List<Mesh.Tri> Tris
        {
            get;
            set;
        }

        public ShapeTriMesh(Q.Scene.Nodes.SceneNode node)
        {

            Vertices = new List<Mesh.Vertex>();
            Tris = new List<Mesh.Tri>();

            foreach(var tri in node.Meshes[0].Tris)
            {

                Vector3  v1, v2, v3;

                v1 = node.Meshes[0].Vertices[tri.V0].Pos;
                v2 = node.Meshes[0].Vertices[tri.V1].Pos;
                v3 = node.Meshes[0].Vertices[tri.V2].Pos;

                Mesh.Vertex vr1, vr2, vr3;

                vr1 = new Mesh.Vertex();
                vr2 = new Mesh.Vertex();
                vr3 = new Mesh.Vertex();

                vr1.Pos = v1;
                vr2.Pos = v2;
                vr3.Pos = v3;
                vr1.Norm = node.Meshes[0].Vertices[tri.V0].Norm;
                vr2.Norm = node.Meshes[0].Vertices[tri.V1].Norm;
                vr3.Norm = node.Meshes[0].Vertices[tri.V2].Norm;

                Vertices.Add(vr1);
                Vertices.Add(vr2);
                Vertices.Add(vr3);


            }

            //Meshes = node.Meshes;


        }
        
    }
}
