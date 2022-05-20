using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Scene;

namespace Q.Elemental
{
    public class Elemental
    {

        public List<EFX> ActiveFX
        {
            get;
            set;
        }

        public SceneGraph CurrentScene
        {
            get;
            set;
        }

        public Elemental()
        {
            ActiveFX = new List<EFX>();
        }

        public void Add(EFX fx)
        {
            ActiveFX.Add(fx);
            fx.Owner = this;
        }

        public void Update()
        {
            foreach(var fx in ActiveFX)
            {
                fx.Update();
            }
        }


    }
}
