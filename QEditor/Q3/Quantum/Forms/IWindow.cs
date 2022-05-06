using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class IWindow : IForm
    {
        
        public IWindowTitle Title
        {
            get;
            set;
        }

        public IWindowContent Content
        {
            get;
            set;
        }

        public int TitleHeight
        {
            get;
            set;
        }

        public IWindow()
        {
            Title = new IWindowTitle();
            Content = new IWindowContent();
            Title.SetText("Window");// ")
            Add(Title, Content);
            TitleHeight = 25;
        }

        public override void Resized()
        {
            Title.Set(0, 0, Size.X+3, TitleHeight);
            Content.Set(0, TitleHeight, Size.X, Size.Y - TitleHeight-1);
            //base.Resized();
        }


    }
}
