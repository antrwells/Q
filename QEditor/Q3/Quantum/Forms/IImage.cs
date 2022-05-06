using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class IImage : IForm
    {

        public Texture.Texture2D Image
        {
            get;
            set;
        }

        public override void RenderForm()
        {
            //base.RenderForm();
            Draw(Image);
        }

        public override bool InBounds(int x,int y)
        {
            return false;
        }

        public void SetImage(Texture.Texture2D image)
        {
            Image = image;   
        }

    }
}
