using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
namespace Q.Physx
{
    public class PXConvexHull : PXBody
    {

        public Mesh.Mesh3D Mesh
        {
            get;
            set;
        }
        public PXConvexHull(Mesh.Mesh3D mesh)
        {
            Mesh = mesh;
            InitBody();
        }

        public override void InitBody()
        {
            //base.InitBody();

            var mesh = Mesh;

            int vc = mesh.Vertices.Count;
            int tc = mesh.Tris.Count;

            //TriangleMeshDesc td = new TriangleMeshDesc();
            ConvexMeshDesc td = new ConvexMeshDesc();


            System.Numerics.Vector3[] points = new System.Numerics.Vector3[vc];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {

                var pos = mesh.Vertices[i].Pos;
                points[i] = new System.Numerics.Vector3(pos.X, pos.Y, pos.Z);

            }

            int[] tris = new int[mesh.Tris.Count * 3];

            int ii = 0;
            for (int i = 0; i < mesh.Tris.Count; i++)
            {

                tris[ii++] = (int)mesh.Tris[i].V0;
                tris[ii++] = (int)mesh.Tris[i].V1;
                tris[ii++] = (int)mesh.Tris[i].V2;

            }

            td.SetPositions(points);
            td.SetTriangles<int>(tris);
            //td.Points = points;
            //td.Triangles = tris
            //t;
            td.Flags = ConvexFlag.ComputeConvex;

            var stream = new MemoryStream();

            var result = Q.Physx.QPhysics._Cooking.CookConvexMesh(td, stream);

            stream.Position = 0;

            var tm = QPhysics._Physics.CreateConvexMesh(stream);

            int bb = 0;



            Material = Q.Physx.QPhysics._Physics.CreateMaterial(0.4f, 0.4f, 0.4f);

            this.DynamicBody = Q.Physx.QPhysics._Physics.CreateRigidDynamic();


            Body = (RigidActor)DynamicBody;
            // var act = (RigidActor)StaticBody;

            Shape = RigidActorExt.CreateExclusiveShape(DynamicBody, new ConvexMeshGeometry(tm), Material);
            int a = 5;

            Q.Physx.QPhysics._Scene.AddActor(Body);


        }

    }
}
