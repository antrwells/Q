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

        public ViewScroller HScroller
        {
            get;
            set;
        }

        public ViewScroller VScroller
        {
            get;
            set;
        }

        public IButton Resizer
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
            //Content.ChildScroll = true;

            Title.SetText("Window");// ")
            HScroller = new ViewScroller();
            VScroller = new ViewScroller();
            Resizer = new IButton();
            Add(Title, Content);
            Add(VScroller, HScroller);
            Add(Resizer);
            
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

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Content.Child.Count < 1) return;
            int max_y = Content.Child[0].ContentSize.Y;
            int max_X = Content.Child[0].ContentSize.X;
            
            VScroller.MaxValue = max_y;
            HScroller.MaxValue = max_X;
            

            //   Console.WriteLine("MX:" + max_X + " MY:" + max_y);
            //int 

            Content.Child[0].ScrollPosition = new OpenTK.Mathematics.Vector2i((int)(HScroller.Value * max_X), (int)(VScroller.Value * max_y));
            // Console.WriteLine("SX:" + HorizontalScroll.Value + " SY:" + VerticalScroll.Value);
            if (Content.Child[0].ScrollPosition.Y < 0)
            {
                Content.Child[0].ScrollPosition = new OpenTK.Mathematics.Vector2i(Content.Child[0].ScrollPosition.X, 0);
            }
            if (Content.Child[0].ScrollPosition.X < 0)
            {
                Content.Child[0].ScrollPosition = new OpenTK.Mathematics.Vector2i(0, Content.Child[0].ScrollPosition.Y);
            }


            //Console.WriteLine("SX:" + ScrollPosition.X);
            // Console.WriteLine("SY:" + ScrollPosition.Y);
            //   Console.Write("AV:" + VerticalScroll.Value);


        }

        public override void OnResized()
        {
            Title.Set(0, 0, Size.X, TitleHeight);
            Content.Set(0, TitleHeight, Size.X-10, Size.Y - TitleHeight-1-12);
            VScroller.Set(Size.X -11, TitleHeight, 10, Size.Y-TitleHeight-12);
            HScroller.Set(0, Size.Y - 10, Size.X-11, 10);
            Resizer.Set(Size.X - 11, Size.Y-12,13,13);
            foreach (var c in Content.Child)
            {
                c.Resized();
            }

            //base.Resized();
        }


    }
}
