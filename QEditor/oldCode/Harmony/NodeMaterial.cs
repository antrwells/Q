using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Harmony
{
    public class NodeMaterial
    {

        public float Bounce
        {
            get;
            set;
        }

        public float Weight
        {
            get;
            set;
        }

        public NodeMaterial()
        {

            Bounce = 0.85f;
            Weight = 1.0f;

        }

    }
}
