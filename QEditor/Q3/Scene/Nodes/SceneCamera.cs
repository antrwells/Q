using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Scene.Nodes
{
    public class SceneCamera : SceneNode
    {

        public override Matrix4 WorldMatrix
        {
            get
            {
                var m = base.WorldMatrix;
                m.Invert();
                return m;
            }
        }

        public float MinDepth
        {
            get;
            set;
        }

        public float MaxDepth
        {
            get;
            set;
        }

        public float FOV
        {
            get;
            set;
        }

        public Matrix4 ProjectionMatrix
        {
            ///Note  - set width/heigh to framewidth/height
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)App.AppInfo.FrameWidth / (float)App.AppInfo.FrameHeight,MinDepth,MaxDepth);  //.CreatePerspectiveOffCen
            }
        }

        public SceneCamera()
        {
            MinDepth = 0.01f;
            MaxDepth = 500.0f;
            FOV = 45.0f;
        }

        public override void Rotate(float pitch, float yaw, float roll = 0)
        {
            LocalRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(pitch)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(yaw));// * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(roll));
            //base.Rotate(pitch, yaw, roll);

        }


    }
}
