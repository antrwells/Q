using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Harmony
{
    
    public class CollisionResult
    {
        
        public bool Collided
        {
            get;
            set;
        }

        public Vector3 CollisionPoint
        {
            get;
            set;
        }

        public float IntersectionLength
        {
            get;
            set;
        }

    }

    public class CollisionTest
    {

        public static CollisionResult Intersect(Shapes.CollisionShape a,Shapes.CollisionShape b)
        {

            if(a is Shapes.ShapeBoundingBox)
            {

                if(b is Shapes.ShapeBoundingBox)
                {

                    return Intersect(a as Shapes.ShapeBoundingBox, b as Shapes.ShapeBoundingBox);

                }

                if(b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapeBoundingBox, b as Shapes.ShapeSphere);

                }

            }

            if(a is Shapes.ShapeSphere)
            {

                if(b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapeSphere, b as Shapes.ShapeSphere);

                }

            }

            return null;

        }

        public static CollisionResult Intersect(Shapes.ShapeSphere a, Shapes.ShapeSphere b)
        {

            CollisionResult res = new CollisionResult();
            res.Collided = false;

            Vector3 d = a.Position - b.Position;

            float dist2 = Vector3.Dot(d, d);

            float radiusSum = a.Radius + b.Radius;

            res.Collided = dist2 <= radiusSum * radiusSum;


            return res;

        }

        public static CollisionResult Intersect(Shapes.ShapeBoundingBox a,Shapes.ShapeSphere b)
        {
            CollisionResult res = new CollisionResult();
            res.Collided = false;

            Vector3 pp = new Vector3();

            var bb = a.GetBounds();
            bb.Min += a.Position;
            bb.Max += a.Position;

            Maths.MathHelp.ClosestPtPointAABB(b.Position,bb, ref pp);

            Vector3 d = pp - b.Position;

            res.CollisionPoint = pp;

            res.Collided =  Vector3.Dot(d, d) <= b.Radius * b.Radius;

            return res;
           
        }

        public static CollisionResult Intersect(Shapes.ShapeBoundingBox a,Shapes.ShapeBoundingBox b)
        {

            var p1 = a.Position;
            var p2 = b.Position;

            var amin = p1 + a.Min;
            var amax = p1+ a.Max;

            var bmin = p2+b.Min;
            var bmax = p2+b.Max;

            CollisionResult res = new CollisionResult();
            res.Collided = false;

        

            if ((amin.X <= bmax.X && amax.X >= bmin.X) && (amin.Y <= bmax.Y && amax.Y >= bmin.Y) && (amin.Z <= bmax.Z && amax.Z >= bmin.Z))
            {
                res.Collided = true;
                res.CollisionPoint = new Vector3(0, 0, 0);
                res.IntersectionLength = 0;
                return res;
            }



            return res;

        }
        public static bool Intersect(Q.Scene.Nodes.BoundingBox a,Q.Scene.Nodes.BoundingBox b)
        {
           // Console.WriteLine("A:" + a.ToString());
         //   Console.WriteLine("B:" + b.ToString());
            return (a.Min.X <= b.Max.X && a.Max.X >= b.Min.X) && (a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y) && (a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z);


        }


    }
}
