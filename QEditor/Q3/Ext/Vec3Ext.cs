using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Ext
{
    public static class Vec3Ext
    {

        public static float Max(this Vector3 val)
        {
            float max = val[0];

            for(int i = 0; i < 3; i++)
            {
                if (val[i]>max)
                {
                    max = val[i];
                }
            }
            return max;

        }

        public static Vector3 Reflect(this Vector3 val,Vector3 other)
        {

            System.Numerics.Vector3 v1 = new System.Numerics.Vector3(val.X, val.Y, val.Z);
            System.Numerics.Vector3 v2 = new System.Numerics.Vector3(other.X, other.Y, other.Z);
            var res = System.Numerics.Vector3.Reflect(v1, v2);
            return new Vector3(res.X, res.Y, res.Z);

        }

    }
}
