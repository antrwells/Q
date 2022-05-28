using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
namespace Q.Physx
{
    public class PXBox : PXBody
    {
        public PXBox(float w,float h,float d)
        {
            W = w / 2;
            H = h / 2;
            D = d / 2;
            InitBody();
        }

        public override void InitBody()
        {
            //base.InitBody();
            Transform tm = new Transform(new System.Numerics.Vector3(0, 0, 0));

            Material = Q.Physx.QPhysics._Physics.CreateMaterial(0.4f, 0.4f, 0.4f);


            this.DynamicBody = Q.Physx.QPhysics._Physics.CreateRigidDynamic();
            Shape = RigidActorExt.CreateExclusiveShape(DynamicBody, new BoxGeometry(W, H, D), Material);
            int a = 5;
            Body = (RigidActor)DynamicBody;
            //DynamicBody.GlobalPose;
            Q.Physx.QPhysics._Scene.AddActor(DynamicBody);


        }

    }
}
