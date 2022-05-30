using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Shader._3D
{
    public class ERimLighting : Effect
    {
        public float RimFactor = 0.5f;
        public OpenTK.Mathematics.Vector3 RimColor = new OpenTK.Mathematics.Vector3(0, 1f, 1f);
        public ERimLighting() : base("engine/shader/rimLightVS.glsl","engine/shader/rimLightFS.glsl")
        {

        }

        public override void BindPars()
        {
            base.BindPars();
            SetUniform("rimFactor", RimFactor);
            SetUniform("rimColor", RimColor);
            SetUniform("normMat", Scene.SceneGlobal.ActiveNode.LocalRotation);
        }

    }
}
