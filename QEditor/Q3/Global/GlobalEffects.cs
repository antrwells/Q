﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Shader;
using Q.Shader._3D;
namespace Q.Global
{
    public class GlobalEffects
    {

        public static EDepth3D DepthFX;
        public static EEmissive3D EmissiveFX;
        public static Q.Shader._2D.EXBasicBlur BlurFX;
        public static Q.Shader._2D.EColorLimit ColorLimitFX;
        public static Q.Shader._2D.EBloom BloomFX;

        public static void Init()
        {
            DepthFX = new EDepth3D();
            EmissiveFX = new EEmissive3D();
            BlurFX = new Shader._2D.EXBasicBlur();
            ColorLimitFX = new Shader._2D.EColorLimit();
            BloomFX = new Shader._2D.EBloom();
            int a = 1;

        }
        
    }
}
