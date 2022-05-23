using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Harmony
{
    public class CollisionTest
    {

        public static bool Intersect(Q.Scene.Nodes.BoundingBox a,Q.Scene.Nodes.BoundingBox b)
        {
           // Console.WriteLine("A:" + a.ToString());
         //   Console.WriteLine("B:" + b.ToString());
            return (a.Min.X <= b.Max.X && a.Max.X >= b.Min.X) && (a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y) && (a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z);


        }

    }
}
