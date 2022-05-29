using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
using OpenTK.Mathematics;
namespace Q.Physx
{
    public class PXTriMesh : PXBody
    {
        public List<Mesh.Mesh3D> Meshes
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public PXTriMesh(List<Mesh.Mesh3D> meshes,int index)
        {
            Meshes = meshes;
            Index = index;
            InitBody();
        }

        public override void InitBody()
        {
            //base.InitBody();

            var mesh = Meshes[Index];

            int vc = mesh.Vertices.Count;
            int tc = mesh.Tris.Count;

            TriangleMeshDesc td = new TriangleMeshDesc();

            System.Numerics.Vector3[] points = new System.Numerics.Vector3[vc];
            for(int i = 0; i < mesh.Vertices.Count; i++)
            {

                var pos = mesh.Vertices[i].Pos;
                points[i] = new System.Numerics.Vector3(pos.X, pos.Y, pos.Z);

            }

            int[] tris = new int[mesh.Tris.Count * 3];

            int ii = 0;
            for(int i = 0; i < mesh.Tris.Count; i++)
            {

                tris[ii++] = (int)mesh.Tris[i].V0;
                tris[ii++] = (int)mesh.Tris[i].V1;
                tris[ii++] = (int)mesh.Tris[i].V2;

            }

            td.Points = points;
            td.Triangles = tris;

            var stream = new MemoryStream();

            var result = Q.Physx.QPhysics._Cooking.CookTriangleMesh(td, stream);

            stream.Position = 0;

            var tm = QPhysics._Physics.CreateTriangleMesh(stream);


            int bb = 0;

            

            Material = Q.Physx.QPhysics._Physics.CreateMaterial(0.4f, 0.4f, 0.4f);

            this.StaticBody = Q.Physx.QPhysics._Physics.CreateRigidStatic();

            Body = (RigidActor)StaticBody;
            // var act = (RigidActor)StaticBody;

            Shape = RigidActorExt.CreateExclusiveShape(StaticBody, new TriangleMeshGeometry(tm), Material);
            int a = 5;

            Q.Physx.QPhysics._Scene.AddActor(Body);


        }


        public TriangleMesh triMesh = null;

    }
}
