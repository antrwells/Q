using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum
{
    public class DragInfo 
    {
        public IForm Form;
        public Texture.Texture2D Icon = null;
        public string Text = "";
        public string Path = "";
        public OpenTK.Mathematics.Vector2i Size = new OpenTK.Mathematics.Vector2i(64, 64);
    }
}
