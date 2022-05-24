using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Scene;

namespace Q.Maths
{
    public class MathHelp
    {
        public static void ClosestPtPointAABB(Vector3 p,Q.Scene.Nodes.BoundingBox b,ref Vector3 point)
        {
            for(int i = 0; i < 3; i++)
            {
                float v = p[i];
                if (v < b.Min[i])
                {
                    v = b.Min[i];
                }
                else if (v > b.Max[i])
                {
                    v = b.Max[i];
                }
                point[i] = v;
            }
        }
        public static float SqrDistPointAABB(Vector3 point,Q.Scene.Nodes.BoundingBox bb)
        {
            float sqrDist = 0.0f;
            

            for(int i = 0; i < 3; i++)
            {
                float v = point[i];
                if (v < bb.Min[i])
                {
                    sqrDist += (bb.Min[i] - v) * (bb.Min[i] - v);
                }
                else if (v > bb.Max[i])
                {
                    sqrDist += (v - bb.Max[i]) * (v - bb.Max[i]);
                }
            }
            return sqrDist;

        }

    }
}
