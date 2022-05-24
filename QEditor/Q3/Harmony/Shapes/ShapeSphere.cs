﻿using Q.Scene.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Harmony.Shapes
{
    public class ShapeSphere : Shapes.CollisionShape
    {

        public float Radius
        {
            get;
            set;
        }

        public override void SetToBounds(SceneNode node)
        {
            //base.SetToBounds(node);

            float fdis = -2000;

            foreach(var mesh in node.Meshes)
            {
                foreach(var vert in mesh.Vertices)
                {

                    var vs = vert.Pos * node.LocalScale;

                    float xd = vs.X;
                    float yd = vs.Y;
                    float zd = vs.Y;

                    float dis = MathF.Sqrt(xd * xd + yd * yd + zd * zd);

                    if (dis > fdis)
                    {
                        fdis = dis;
                    }

                }

                

            }

            Radius = fdis * 2;

        }

    }
}
