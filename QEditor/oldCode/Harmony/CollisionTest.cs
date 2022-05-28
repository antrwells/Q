using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Ext;
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

        public Vector3 CollisionNormalA
        {
            get;
            set;
        }

        public Vector3 CollisionNormalB
        {
            get;
            set;
        }

        public PhysicsNode Node1
        {
            get;
            set;
        }

        public PhysicsNode Node2
        {
            get;
            set;
        }

        public float IntersectionLength
        {
            get;
            set;
        }

        public Vector3 Direction
        {
            get;
            set;
        }

        public float Distance
        {
            get
            {
                return Direction.Length;
            }
        }

    }

    public class CollisionTest
    {

        public static CollisionResult Intersect(Shapes.CollisionShape a, Shapes.CollisionShape b)
        {
            if(a is Shapes.ShapePlane)
            {

                if(b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapePlane, b as Shapes.ShapeSphere);

                }

            }
            if (a is Shapes.ShapeBoundingBox)
            {

                if (b is Shapes.ShapeBoundingBox)
                {

                    return Intersect(a as Shapes.ShapeBoundingBox, b as Shapes.ShapeBoundingBox);

                }

                if (b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapeBoundingBox, b as Shapes.ShapeSphere);

                }

            }

            if (a is Shapes.ShapeSphere)
            {

                if (b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapeSphere, b as Shapes.ShapeSphere);

                }

            }


            if (a is Shapes.ShapeTriMesh)
            {
                if (b is Shapes.ShapeSphere)
                {

                    return Intersect(a as Shapes.ShapeTriMesh, b as Shapes.ShapeSphere);

                }
                if(b is Shapes.ShapeBoundingBox)
                {

                    return Intersect(a as Shapes.ShapeTriMesh, b as Shapes.ShapeBoundingBox);
                }

            }

            return null;

        }


        private static bool TestSphereTri(Shapes.ShapeSphere s, Vector3 a, Vector3 b, Vector3 c, ref Vector3 p)
        {

            p = Maths.MathHelp.ClosestPtPointTriangle(s.Position, a, b, c);

            Vector3 v = p - s.Position;
            return (Vector3.Dot(v, v) <= s.Radius * s.Radius);

        }


        static bool planeBoxOverlap(Vector3 normal, Vector3 vert, Vector3 maxbox)    // -NJMP-

        {

            int q;

            Vector3 vmin, vmax;
            
            vmin = new Vector3();
            vmax = new Vector3();

            float v;

            for (q = 0; q <= 2; q++)

            {

                v = vert[q];                    // -NJMP-

                if (normal[q] > 0.0f)

                {

                    vmin[q] = -maxbox[q] - v;   // -NJMP-

                    vmax[q] = maxbox[q] - v;    // -NJMP-

                }

                else

                {

                    vmin[q] = maxbox[q] - v;    // -NJMP-

                    vmax[q] = -maxbox[q] - v;   // -NJMP-

                }

            }

            if (Vector3.Dot(normal, vmin) > 0.0f) return false; // -NJMP-

            if (Vector3.Dot(normal, vmax) >= 0.0f) return true;    // -NJMP-



            return false;

        }


        
        static bool triBoxOverlap(Vector3 boxcenter, Vector3 boxhalfsize, Vector3[] triverts)
        {

          

            Vector3 v0, v1, v2;
            v0 = new Vector3();
            v1 = new Vector3();
            v2 = new Vector3();

            float min, max, p0, p1, p2, rad, fex, fey, fez;     // -NJMP- "d" local variable removed

            Vector3 normal, e0, e1, e2;

            v0 = triverts[0] - boxcenter;
            v1 = triverts[1] - boxcenter;
            v2 = triverts[2] - boxcenter;

            e0 = v1 - v0;
            e1 = v2 - v1;
            e2 = v0 - v2;

            fex = Math.Abs(e0[0]);

            fey = Math.Abs(e0[1]);

            fez = Math.Abs(e0[2]);

            void FindMinMax(float x0, float x1, float x2)
            {
                min = max = x0;
                if (x1 < min) min = x1;
                if (x1 > max) max = x1;
                if (x2 < min) min = x2;
                if (x2 > max) max = x2;
            }

            int AXISTEST_X01(float a,float b,float fa,float fb)
            {
                p0 = a * v0[1] - b * v0[2];
                p2 = a * v2[1] - b * v2[2];
                if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }
                rad = fa * boxhalfsize[1] + fb * boxhalfsize[2];
                if (min > rad || max < -rad) return 0;
                return 1;

            }

            int AXISTEST_X2(float a,float b,float fa,float fb)
            {

                p0 = a * v0[1] - b * v0[2];
                p1 = a * v1[1] - b * v1[2];
                if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
                rad = fa * boxhalfsize[1] + fb * boxhalfsize[2];
                if (min > rad || max < -rad) return 0;
                return 1;

            }

            int AXISTEST_Y02(float a, float b, float fa, float fb)
            {

                p0 = -a * v0[0] + b * v0[2];
                p2 = -a * v2[0] + b * v2[2];
                if (p0 < p2) { min = p0; max = p2; } else { min = p2; max = p0; }
                rad = fa * boxhalfsize[0] + fb * boxhalfsize[2];
                if (min > rad || max < -rad) return 0;
                return 1;

            }

            int AXISTEST_Y1(float a,float b,float fa,float fb)
            {
                p0 = -a * v0[0] + b * v0[2];
                p1 = -a * v1[0] + b * v1[2];
                if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
                rad = fa * boxhalfsize[0] + fb * boxhalfsize[2];
                if (min > rad || max < -rad) return 0;
                return 1;

            }

            int AXISTEST_Z12(float a,float b,float fa,float fb)
            {

                p1 = a * v1[0] - b * v1[1];
                p2 = a * v2[0] - b * v2[1];
                if (p2 < p1) { min = p2; max = p1; } else { min = p1; max = p2; }
                rad = fa * boxhalfsize[0] + fb * boxhalfsize[1];
                if (min > rad || max < -rad) return 0;
                return 1;
            }

            int AXISTEST_Z0(float a,float b,float fa,float fb)
            {

                p0 = a * v0[0] - b * v0[1];
                p1 = a * v1[0] - b * v1[1];
                if (p0 < p1) { min = p0; max = p1; } else { min = p1; max = p0; }
                rad = fa * boxhalfsize[0] + fb * boxhalfsize[1];
                if (min > rad || max < -rad) return 0;
                return 1;

            }


            if (AXISTEST_X01(e0[2], e0[1], fez, fey) == 0)
            {
                return false;
            }
            if (AXISTEST_Y02(e0[2], e0[0], fez, fex) == 0)
            {
                return false;
            }
            if (AXISTEST_Z12(e0[1], e0[0], fey, fex) == 0)
            {
                return false;
            }


            fex = Math.Abs(e1[0]);

            fey = Math.Abs(e1[1]);

            fez = Math.Abs(e1[2]);

            if (AXISTEST_X01(e1[2], e1[1], fez, fey) == 0)
            {
                return false;
            }
            if (AXISTEST_Y02(e1[2], e1[0], fez, fex) == 0)
            {
                return false;
            }
            if (AXISTEST_Z0(e1[1], e1[0], fey, fex) == 0)
            {
                return false;
            }

            fex = Math.Abs(e2[0]);

            fey = Math.Abs(e2[1]);

            fez = Math.Abs(e2[2]);


            if (AXISTEST_X2(e2[2], e2[1], fez, fey) == 0)
            {
                return false;
            }
            if (AXISTEST_Y1(e2[2], e2[0], fez, fex) == 0)
            {
                return false;
            }
            if (AXISTEST_Z12(e2[1], e2[0], fey, fex) == 0)
            {
                return false;
            }


            FindMinMax(v0[0], v1[0], v2[0]);

            if (min > boxhalfsize[0] || max < -boxhalfsize[0]) return false;

            FindMinMax(v0[1], v1[1], v2[1]);

            if (min > boxhalfsize[1] || max < -boxhalfsize[1]) return false;

            FindMinMax(v0[2], v1[2], v2[2]);

            if (min > boxhalfsize[2] || max < -boxhalfsize[2]) return false;

            normal = Vector3.Cross(e0, e1);

            if (!planeBoxOverlap(normal, v0, boxhalfsize)) return false;

            return true;


            //SUB(v0, triverts[0], boxcenter);

            //SUB(v1, triverts[1], boxcenter);

            //SUB(v2, triverts[2], boxcenter);

            return false;

        }
        

        /*
        private static bool TestTriangleAABB(Vector3 v0,Vector3 v1,Vector3 v2,Shapes.ShapeBoundingBox b)
        {

            float p0, p1, p2, r;

            Vector3 c = (b.Min + b.Max) * 0.5f;

            float e0 = (b.Max.X - b.Min.X) * 0.5f;
            float e1 = (b.Max.Y - b.Min.Y) * 0.5f;
            float e2 = (b.Max.Z - b.Min.Z) * 0.5f;

            v0 = v0 - c;
            v1 = v1 - c;
            v2 = v2 - c;

            Vector3 f0 = v1 - v0, f1 = v2 - v1, f2 = v0 - v2;

            p0 = v0.Z * v1.Y - v0.Y * v1.Z;
            p2 = v2.Z * (v1.Y - v0.Y) - v2.Z * (v1.Z - v0.Z);
            r = e1 * Math.Abs(f0.Z) + e2 * Math.Abs(f0.Y);
            if (Math.Max(-Math.Max(p0, p2), Math.Min(p0, p2)) > r) return false;




        }
        */

        public static CollisionResult Intersect(Shapes.ShapePlane a,Shapes.ShapeSphere b)
        {

            CollisionResult res = new CollisionResult();
            res.Collided = false;

            float distance = Math.Abs(Vector3.Dot(a.Normal, b.Position)+a.Distance);
            float distanceSphere = distance - b.Radius;

            if(distanceSphere<0)
            {
                res.Collided = true;
                //res.Distance = a.Normal * distanceSphere;
                res.Direction = a.Normal * distanceSphere;
                res.Node1 = a.Node;
                res.Node2 = b.Node;
                res.CollisionNormalA = (b.Position - a.Position).Normalized();
                res.CollisionNormalB = -res.CollisionNormalA;
                return res;
            }


            return res;

        }

        public static CollisionResult Intersect(Shapes.ShapeTriMesh a,Shapes.ShapeBoundingBox b)
        {

            CollisionResult res = new CollisionResult();

            res.Collided = false;


            Vector3 cp = new Vector3(20000, 20000, 2000);
            Vector3 cn = new Vector3(0, 0, 0);
            float cd = 200000;

            for (int v = 0; v < a.Vertices.Count; v += 3)
            {

                Vector3 v0, v1, v2;

                v0 = a.Vertices[v].Pos;
                v1 = a.Vertices[v + 1].Pos;
                v2 = a.Vertices[v + 2].Pos;

                Vector3[] verts = new Vector3[3];
                verts[0] = v0;
                verts[1] = v1;
                verts[2] = v2;

                Vector3 hp = new Vector3(0, 0, 0);

                Vector3 bh = b.Max - b.Min;
                bh = bh / 2;

                if (triBoxOverlap(b.Position,bh,verts))
                {


                    float dis = Vector3.DistanceSquared(hp, b.Position);
                    //if (dis < cd)
                    //{
                        cd = dis;
                        cp = hp;
                        cn = a.Vertices[v].Norm;
                    //}


                    res.Collided = true;




                }


            }


            res.CollisionPoint = cp;
            if (res.Collided)
            {
                res.CollisionNormalA = cn;
            }

            return res;

        }

        public static CollisionResult Intersect(Shapes.ShapeTriMesh a,Shapes.ShapeSphere b)
        {

            CollisionResult res = new CollisionResult();
            res.Collided = false;

            Vector3 cp = new Vector3(20000, 20000, 2000);
            Vector3 cn = new Vector3(0, 0, 0);
            float cd = 200000;

            for (int v = 0; v < a.Vertices.Count; v += 3)
            {

                Vector3 v0, v1, v2;

                v0 = a.Vertices[v].Pos;
                v1 = a.Vertices[v + 1].Pos;
                v2 = a.Vertices[v + 2].Pos;

                Vector3 hp = new Vector3(0, 0, 0);

                if (TestSphereTri(b, v0, v1, v2, ref hp))
                {

                    float dis = Vector3.DistanceSquared(hp, b.Position);
                    if (dis < cd)
                    {
                        cd = dis;
                        cp = hp;
                        cn = a.Vertices[v].Norm;
                    }

                    res.Collided = true;




                }


            }        

            
            res.CollisionPoint = cp;
            res.CollisionNormalA = cn;

            return res;

        }

        public static CollisionResult Intersect(Shapes.ShapeSphere a, Shapes.ShapeSphere b)
        {

            CollisionResult res = new CollisionResult();
            res.Collided = false;

            Vector3 d = a.Position - b.Position;

            float dist2 = Vector3.Dot(d, d);

            float radiusSum = a.Radius + b.Radius;
            Vector3 direction = (b.Position - a.Position);
            float centreDistance = direction.Length;
            direction /= centreDistance;
            float distance = centreDistance - radiusSum;

            if(centreDistance < radiusSum)
            {
                res.Direction = direction * distance;
                res.Collided = true;
                res.CollisionNormalA = -d.Normalized();
                res.CollisionNormalB = -res.CollisionNormalA;
                res.Node1 = a.Node;
                res.Node2 = b.Node;
                return res;
            }

            //res.Collided = dist2 <= radiusSum * radiusSum;

            //res.CollisionNormal = -d.Normalized();



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

            Vector3 distances1 = bmin - amax;
            Vector3 distances2 = amin - bmax;
            Vector3 distances = Vector3.ComponentMax(distances1, distances2);

            float maxDist = distances.Max();

            CollisionResult res = new CollisionResult();
            res.Collided = false;



            if (maxDist < 0)
            {

                res.Collided = true;
                res.Direction = distances;
                return res;

            }
            
            
         

            if ((amin.X <= bmax.X && amax.X >= bmin.X) && (amin.Y <= bmax.Y && amax.Y >= bmin.Y) && (amin.Z <= bmax.Z && amax.Z >= bmin.Z))
            {
                res.Collided = true;
                res.CollisionPoint = new Vector3(0, 0, 0);
                res.IntersectionLength = 0;
                res.CollisionNormalA = b.Position - a.Position;

                float ln = res.CollisionNormalA.LengthSquared;

                res.CollisionNormalA /= ln;

                //res.CollisionNormal


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
