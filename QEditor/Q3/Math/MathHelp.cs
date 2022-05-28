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

        public static Vector3 ClosestPtPointTriangle(Vector3 p,Vector3 a,Vector3 b,Vector3 c)
        {

            Vector3 ab = b - a;
            Vector3 ac = c - a;
            Vector3 ap = p - a;
            float d1 = Vector3.Dot(ab, ap);
            float d2 = Vector3.Dot(ac, ap);
            if (d1 <= 0.0f && d2 <= 0.0f) return a;

            // Check if P in vertex region outside B
            Vector3 bp = p - b;
            float d3 = Vector3.Dot(ab, bp);
            float d4 = Vector3.Dot(ac, bp);
            if (d3 >= 0.0f && d4 <= d3) return b; // barycentric coordinates (0,1,0)
                                                  // Check if P in edge region of AB, if so return projection of P onto AB
            float vc = d1 * d4 - d3 * d2;
            if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f)
            {
                float v1 = d1 / (d1 - d3);
                return a + v1 * ab; // barycentric coordinates (1-v,v,0)
            }
            // Check if P in vertex region outside C
            Vector3 cp = p - c;
            float d5 = Vector3.Dot(ab, cp);
            float d6 = Vector3.Dot(ac, cp);
            if (d6 >= 0.0f && d5 <= d6) return c; // barycentric coordinates (0,0,1)142 Chapter 5 Basic Primitive Tests
                                                  // Check if P in edge region of AC, if so return projection of P onto AC
            float vb = d5 * d2 - d1 * d6;
            if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f)
            {
                float w1 = d2 / (d2 - d6);
                return a + w1 * ac; // barycentric coordinates (1-w,0,w)
            }
            // Check if P in edge region of BC, if so return projection of P onto BC
            float va = d3 * d6 - d5 * d4;
            if (va <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f)
            {
                float w2 = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + w2 * (c - b); // barycentric coordinates (0,1-w,w)
            }
            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            float denom = 1.0f / (va + vb + vc);
            float v = vb * denom;
            float w = vc * denom;
            return a + ab * v + ac * w;


        }

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
