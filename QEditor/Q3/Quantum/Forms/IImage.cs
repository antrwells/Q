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

        public override void CompleteDrop(DragInfo info)
        {
            //base.CompleteDrop(info);
            if(File.Exists(info.Path))
            {
                Image = new Texture.Texture2D(info.Path);
            }
        }

        public IImage()
        {
            AcceptDrops = true;
        }
        public override void RenderForm()
        {
            //base.RenderForm();
            Draw(Image);
        }

        public override bool InBounds(int x,int y)
        {
            return base.InBounds(x, y);
          //  return true;
        }

        public void SetImage(Texture.Texture2D image)
        {
            Image = image;   
        }

    }
}
