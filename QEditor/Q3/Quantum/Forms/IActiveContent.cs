using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum.Forms;

namespace Q.Quantum.Forms
{
    public class IActiveContent : IForm
     {

        private bool resizeLeft, resizeRight, resizeBottom, resizeTop, resizeCorner;
        private bool resizing = false;

        public IToolBar ToolBar
        {
            get
            {
                return _ToolBar;
            }
            set
            {
                _ToolBar = value;
                Add(_ToolBar);
                _ToolBar.Set(0, 0, Size.X, 45);
            }                
        }
        private IToolBar _ToolBar = null;

        public ViewScroller VerticalScroll
        {
            get;
            set;
        }
        
        public ViewScroller HorizontalScroll
        {
            get;
            set;
        }
        

        public IActiveContent()
        {
            resizeLeft = resizeRight = resizeBottom = resizeTop = resizeCorner = false;
            Color = new OpenTK.Mathematics.Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            ChildScroll = true;
            VerticalScroll = new ViewScroller();
            VerticalScroll.Scroll = false;
            HorizontalScroll = new ViewScroller();
            HorizontalScroll.Scroll = false;
            HorizontalScroll.Horizontal = true;
            Add(HorizontalScroll);
            _ToolBar = null;

            
            Add(VerticalScroll);
       
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            int max_y = ContentSize.Y;
            int max_X = ContentSize.X;

            VerticalScroll.MaxValue = max_y;
            HorizontalScroll.MaxValue = max_X;

         //   Console.WriteLine("MX:" + max_X + " MY:" + max_y);
            //int 

            ScrollPosition = new OpenTK.Mathematics.Vector2i((int)(HorizontalScroll.Value*max_X),(int)(VerticalScroll.Value * max_y));
           // Console.WriteLine("SX:" + HorizontalScroll.Value + " SY:" + VerticalScroll.Value);
            if (ScrollPosition.Y<0)
            {
                ScrollPosition = new OpenTK.Mathematics.Vector2i(ScrollPosition.X, 0);
            }
            if (ScrollPosition.X < 0)
            {
                ScrollPosition = new OpenTK.Mathematics.Vector2i(0, ScrollPosition.Y);
            }

            
            //Console.WriteLine("SX:" + ScrollPosition.X);
           // Console.WriteLine("SY:" + ScrollPosition.Y);
         //   Console.Write("AV:" + VerticalScroll.Value);

            
        }

        public override void OnResized()
        {
            VerticalScroll.Set(Size.X - 10, 0, 10, Size.Y - 10);
            HorizontalScroll.Set(0, Size.Y - 10, Size.X - 10, 10);
            if (ToolBar != null)
            {
                ToolBar.Set(0, 0, Size.X, 45);
            }
        }

        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            base.OnMouseMove(x, y, x_delta, y_delta);
            if (Input.AppInput.MouseButton[0] == false)
            {
                resizeTop = false;
                resizeLeft = false;
                resizeRight = false;
                resizeBottom = false;
                resizeCorner = false;
            }
            if (!resizing)
            {
                if (Within(x, y, RenderPosition.X, RenderPosition.Y, 5, Size.Y))
                {
                    resizeLeft = true;
                    resizeTop = false;
                    resizeBottom = false;
                    resizeRight = false;
                    resizeCorner = false;
                }
                if (Within(x, y, RenderPosition.X, RenderPosition.Y + Size.Y - 5, Size.X - 8, 5))
                {
                    resizeBottom = true;
                    resizeTop = false;
                    resizeLeft = false;
                    resizeRight = false;
                    resizeCorner = false;
                }
                if (Within(x, y, RenderPosition.X + Size.X - 5, RenderPosition.Y, 5, Size.Y - 8))
                {
                    resizeRight = true;
                    resizeTop = false;
                    resizeBottom = false;
                    resizeCorner = false;
                    resizeLeft = false;
                }
                if (Within(x, y, RenderPosition.X, RenderPosition.Y, Size.X, 5))
                {
                   // Console.WriteLine("Within Top");
                    resizeTop = true;
                    resizeBottom = false;
                    resizeLeft = false;
                    resizeRight = false;
                    resizeCorner = false;
                }
                if (Within(x, y, RenderPosition.X + Size.X - 8, RenderPosition.Y + Size.Y - 8, 8, 8))
                {
                    resizeCorner = true;
                    resizeTop = false;
                    resizeBottom = false;
                    resizeLeft = false;
                    resizeRight = false;


                }

            }

            if (resizing)
            {

              

                if (resizeLeft)
                {

                    Root.Position = new OpenTK.Mathematics.Vector2i(Root.Position.X + x_delta, Root.Position.Y);
                    Root.Size = new OpenTK.Mathematics.Vector2i(Root.Size.X - x_delta, Root.Size.Y);

                }else 
                if(resizeBottom)
                {
                    Root.Size = new OpenTK.Mathematics.Vector2i(Root.Size.X, Root.Size.Y + y_delta);
                }else
                if (resizeRight)
                {
                    Root.Size = new OpenTK.Mathematics.Vector2i(Root.Size.X + x_delta, Root.Size.Y);
                }else
                if (resizeTop)
                {
                   // Console.WriteLine("!!!!!!!!!!!!");
;                    Root.Position = new OpenTK.Mathematics.Vector2i(Root.Position.X, Root.Position.Y + y_delta);
                    Root.Size = new OpenTK.Mathematics.Vector2i(Root.Size.X, Root.Size.Y - y_delta);
                }
                if (resizeCorner)
                {
                    Root.Size = new OpenTK.Mathematics.Vector2i(Root.Size.X + x_delta, Root.Size.Y + y_delta);
                }

            }
            
        }

        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            if (resizeLeft || resizeRight || resizeBottom || resizeTop || resizeCorner)
            {
                resizing = true;
                //Console.WriteLine("Mouse Down Window.");
            }
            if(Root.Root is IWindow)
            {
                Root.Root.Root.Child.Remove(Root.Root);
                Root.Root.Root.Child.Add(Root.Root);
            }
            
        }

        public override void OnMouseUp(int button)
        {
            base.OnMouseUp(button);
            //Console.WriteLine("Reset reszie!!!");
            resizing = false;
            //resizeLeft = resizeRight = resizeBottom = resizeTop = false;
            resizeLeft = false;
            resizeRight = false;
            resizeTop = false;
            resizeCorner = false;
            resizeBottom = false;
           // Console.WriteLine("Mouse up window.");
        }

        public override void RenderForm()
        {

            Child.Remove(VerticalScroll);
            Child.Remove(HorizontalScroll);
            if(true)
            {
                Add(HorizontalScroll);
                HorizontalScroll.Position = new OpenTK.Mathematics.Vector2i(0,Size.Y - 15);
                HorizontalScroll.Size = new OpenTK.Mathematics.Vector2i(Size.X,15);
            };
            if (true)
            {
                
                Add(VerticalScroll);
              //  Console.WriteLine("ADDING SCROLLER");
                VerticalScroll.Position = new OpenTK.Mathematics.Vector2i(Size.X - 15, 0);
                VerticalScroll.Size = new OpenTK.Mathematics.Vector2i(15, Size.Y);
            }
            else
            {
               // ScrollPosition = new OpenTK.Mathematics.Vector2i(ScrollPosition.X, 0);
            }
                if (ToolBar != null)
            {
                Child.Remove(ToolBar);
                Add(ToolBar);
            }

            
            //base.RenderForm();
            Color = new OpenTK.Mathematics.Vector4(1, 1, 1, 0.7f);
            DrawBlur(RenderPosition.X, RenderPosition.Y, Size.X, Size.Y);



            DrawFrame(new OpenTK.Mathematics.Vector4(1, 1, 1, 0.5f));
            Color = new OpenTK.Mathematics.Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            DrawLine(RenderPosition.X-1, RenderPosition.Y, RenderPosition.X-1 + Size.X, RenderPosition.Y, Color);
            DrawLine(RenderPosition.X-1, RenderPosition.Y, RenderPosition.X-1, RenderPosition.Y + Size.Y,Color);
            DrawLine(RenderPosition.X-1, RenderPosition.Y+Size.Y, RenderPosition.X-1+Size.X, RenderPosition.Y + Size.Y,Color);
            DrawLine(RenderPosition.X-1+Size.X, RenderPosition.Y, RenderPosition.X-1+Size.X, RenderPosition.Y + Size.Y,Color);

          //  DrawFrame(RenderPosition.X, RenderPosition.Y, ContentSize.X, ContentSize.Y, new OpenTK.Mathematics.Vector4(1, 1, 1, 0.7f));

        }

    }
}
