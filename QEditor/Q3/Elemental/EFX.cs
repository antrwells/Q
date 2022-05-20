using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Elemental
{
    public class EFX
    {
        
        public Elemental Owner
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public virtual void Init()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }

    }
}
