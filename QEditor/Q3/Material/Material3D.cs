using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Texture;
using OpenTK.Mathematics;
namespace Q.Material
{
    public class Material3D
    {
        public static Texture2D DefaultColor, DefaultNormal, DefaultSpecular, DefaultEmissive; 
            
        public Texture2D ColorMap
        {
            get;
            set;
        }

        public Texture2D NormalMap
        {
            get;
            set;
        }

        public Texture2D SpecularMap
        {
            get;
            set;
        }

        public Texture2D EmissiveMap
        {
            get;
            set;
        }


        public Vector3 Diffuse
        {
            get;
            set;
        }

        public Vector3 Specular
        {
            get;
            set;
        }
        public Material3D()
        {
            if (DefaultColor == null)
            {
                DefaultColor = new Texture2D("data/tex/diffblank.png");
                DefaultNormal = new Texture2D("data/tex/normBlank.png");
                DefaultSpecular = new Texture2D("data/tex/specBlank.png");
                DefaultEmissive = new Texture2D("data/tex/emissiveBlank.png");
            }
            ColorMap = DefaultColor; 
            NormalMap = DefaultNormal;
            SpecularMap = DefaultSpecular;
            EmissiveMap = DefaultEmissive;
            
        }

    }
}
