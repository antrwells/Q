using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
namespace Q.Physx
{
    public class PXSphere : PXBody
    {
        public float Radius
        {
            get;
            set;
        }
        public PXSphere(float size)
        {
            Radius = size;
            InitBody();
        }

        public override void InitBody()
        {
            //base.InitBody();
            Material = Q.Physx.QPhysics._Physics.CreateMaterial(0.4f, 0.4f, 0.4f);


            this.DynamicBody = Q.Physx.QPhysics._Physics.CreateRigidDynamic();
            Shape = RigidActorExt.CreateExclusiveShape(DynamicBody, new SphereGeometry(Radius), Material);
            int a = 5;
            Body = (RigidActor)DynamicBody;
            //DynamicBody.GlobalPose;
            Q.Physx.QPhysics._Scene.AddActor(DynamicBody);
        }
    }
}
