using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
namespace Q.Physx
{
    public class PXBody
    {

        public RigidStatic StaticBody
        {
            get;
            set;
        }

        public RigidDynamic DynamicBody
        {
            get;
            set;
        }

        public RigidActor Body
        {
            get;
            set;
        }
        

        public Shape Shape
        {
            get;
            set;
        }

        public PhysX.Material Material
        {
            get;
            set;
        }

        public PXBody()
        {

          //  InitBody();

        }

        //private OpenTK.Mathematics.Matrix4 

        public void SetPose(OpenTK.Mathematics.Vector3 pos,OpenTK.Mathematics.Matrix4 rot)
        {

            var pose = System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(pos.X, pos.Y, pos.Z));

            Body.GlobalPose = pose;
       
        }

        public void MakeStatic()
        {

           //cBody.RigidDynamicLockFlags = RigidDynamicLockFlags.LinearX | RigidDynamicLockFlags.LinearY | RigidDynamicLockFlags.LinearZ | RigidDynamicLockFlags.AngularX | RigidDynamicLockFlags.AngularY | RigidDynamicLockFlags.AngularZ;
            //DynamicBody.RigidDynamicLockFlags 

        }

        public virtual void InitBody()
        {

        }

        public OpenTK.Mathematics.Vector3 GetPos()
        {

            var pose = Body.GlobalPosePosition;
            return new OpenTK.Mathematics.Vector3(pose.X, pose.Y, pose.Z);

        }

        public OpenTK.Mathematics.Matrix4 GetRot()
        {

            var rot = Body.GlobalPoseQuat;
            OpenTK.Mathematics.Quaternion q = new OpenTK.Mathematics.Quaternion(rot.X, rot.Y, rot.Z, rot.W);

            return OpenTK.Mathematics.Matrix4.CreateFromQuaternion(q);

        }

        public float W, H, D;

    }
}
