using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Harmony.Shapes
{
    public class ShapePlane : CollisionShape 
    {

        public Vector3 Normal
        {
            get;
            set;
        }

        public float Distance
        {
            get;
            set;
        }

        public ShapePlane Normalized
        {
            get
            {
                float mag = Normal.Length;
                return new ShapePlane(Normal / mag, Distance / mag);
            }
        }

        public ShapePlane(Vector3 normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }
    


    }
}
