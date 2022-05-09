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

        public IGroup Content
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
            Content = new IGroup();
            Title.SetText("Window");// ")
            Add(Title, Content);
            TitleHeight = 25;
        }

        public bool TitleOn
        {
            get
            {
                return _TitleOn;
            }
            set
            {
                if (value)
                {
                    Add(Title);
                    Position = new OpenTK.Mathematics.Vector2i(Position.X, Position.Y + TitleHeight);
                }
                else
                {
                    Child.Remove(Title);
                    Position = new OpenTK.Mathematics.Vector2i(Position.X, Position.Y - TitleHeight);
                }
                _TitleOn = value;
            }
        }
        private bool _TitleOn = true;

        public override void OnResized()
        {
            Title.Set(0, 0, Size.X+3, TitleHeight);
            Content.Set(0, TitleHeight, Size.X, Size.Y - TitleHeight-1);
            //base.Resized();
        }


    }
}
