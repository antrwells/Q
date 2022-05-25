using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Scene.Nodes;

namespace Q.Harmony.Shapes
{
    public class ShapeBoundingBox : CollisionShape
    {

        public Vector3 Min
        {
            get;
            set;
        }

        public Vector3 Max
        {
            get;
            set;
        }

        public override BoundingBox GetBounds()
        {

            Q.Scene.Nodes.BoundingBox bb = new BoundingBox();

            bb.Min = Min;
            bb.Max = Max;

            return bb;
            
            //return base.GetBounds();

        }

        public override void SetToBounds(Q.Scene.Nodes.SceneNode node)
        {
            var bb = node.GetBounds();

            var size = new Vector3();

            size.X = bb.Max.X - bb.Min.X;
            size.Y = bb.Max.Y - bb.Min.Y;
            size.Z = bb.Max.Z - bb.Min.Z;



            //         SBB = new Shapes.ShapeBoundingBox();

            //       SBB.Min = bb.Min;
            //     SBB.Max = bb.Max;
            //   Shape = SBB;
            Min = bb.Min;
            Max = bb.Max;


        }


    }
}
