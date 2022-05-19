using System;
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

        public static EAnimMesh AnimMeshFX;
        public static EDepth3D DepthFX;
        public static EEmissive3D EmissiveFX;
        public static Q.Shader._2D.EXBasicBlur BlurFX;
        public static Q.Shader._2D.EColorLimit ColorLimitFX;
        public static Q.Shader._2D.EBloom BloomFX;
        public static Q.Shader._3D.EDepthAnimMesh DepthAnimMeshFX;

        public static void Init()
        {
            DepthFX = new EDepth3D();
            EmissiveFX = new EEmissive3D();
            BlurFX = new Shader._2D.EXBasicBlur();
            ColorLimitFX = new Shader._2D.EColorLimit();
            BloomFX = new Shader._2D.EBloom();
            AnimMeshFX = new EAnimMesh();
            DepthAnimMeshFX = new EDepthAnimMesh();
            int a = 1;

        }
        
    }
}
