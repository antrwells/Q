using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Texture;
using OpenTK.Graphics.OpenGL;


namespace Q.Quantum
{
    public class IForm
    {

        public IForm Root
        {
            get;
            set;
        }

        public List<IForm> Child
        {
            get;
            set;
        }

        public Vector2i Position
        {
            get;
            set;
        }



        public Vector2i Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
                Resized();
            }
        }
        private Vector2i _Size = new Vector2i(0, 0);

        public Vector4 Color
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public bool Focus
        {
            get;
            set;
        }

        public bool Over
        {
            get;
            set;
        }

        public bool Drag
        {
            get;
            set;
        }

        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
            }
        }
        private string _Text = "";

        public Vector2i ScrollPosition
        {
            get;
            set;
        }

        public bool Scroll
        {
            get;
            set;
        }

        public Vector2i ContentSize
        {

            get
            {

                int bx = 0;
                int by = 0;

                foreach(var form in Child)
                {
                    var x = form.Position.X+ (form.Size.X-Size.X);
                    var y = form.Position.Y+ (form.Size.Y-Size.Y);
                                     
                  
                    // x = x - Size.X/2;
                    //y = y - Size.Y;
                    
//                    x = form.Position.X 
                
                    
                    if (x > bx) bx = x;
                    if (y > by) by = y;
                }


               
                return new Vector2i(bx, by);

            }
            
        }

        public Vector2i RenderPosition
        {
            get
            {
                Vector2i pos = new Vector2i(0, 0);
                if (Root != null)
                {
                    pos = Root.RenderPosition;
                    if (Root.ChildScroll && Scroll)
                    {
                        pos.X -= Root.ScrollPosition.X;
                        pos.Y -= Root.ScrollPosition.Y;
                    }
                
                }
                
                return pos + Position;
            }
        }

        public Vector2i RenderPositionNoScroll
        {
            get
            {
                Vector2i pos = new Vector2i(0, 0);
                if (Root != null)
                {
                    pos = Root.RenderPosition;
                }
                return pos + Position;
            }
        }

        public bool ChildScroll
        {
            get;
            set;
        }
       

        public IForm()
        {

            Root = null;
            Child = new List<IForm>();
            Scroll = true;
            ChildScroll = false;
           // Set(0, 0, 0, 0);
            SetText("");
            SetColor(1, 1, 1, 1);

        }

        public IForm SetColor(float r,float g,float b,float a)
        {
            SetColor(new Vector4(r, g, b, a));
            return this;
        }
        public IForm SetColor(Vector4 color)
        {
            Color = color;
            return this;
        }

        public IForm Set(int x, int y, int w, int h)
        {
            Position = new Vector2i(x, y);
            Size = new Vector2i(w, h);
            Resized();
            return this;
        }

        public IForm SetText(string text)
        {
            Text = text;
            Renamed();
            return this;
        }

        public IForm Add(params IForm[] forms)
        {
            foreach (var form in forms)
            {
                Add(form);
            }
            return forms[0];
        }

        public IForm Add(IForm form)
        {

            form.Root = this;
            Child.Add(form);
            return this;
        }

        public void Render()
        {

            RenderForm();
            RenderChildren();

        }

        public virtual void RenderForm()
        {

        }

        public void RenderChildren()
        {
            //GL.Viewport(RenderPosition.X, RenderPosition.Y, Size.X, Size.Y);
            //GL.Viewport(0, RenderPosition.Y-Size.Y, App.AppInfo.Width, Size.Y);
        


            if (this is Forms.IWindow || this is Forms.IWindowContent)
            {
                int ty = App.AppInfo.Height - (RenderPosition.Y + Size.Y);

                int sy = App.AppInfo.Height - ty;

                GL.Enable(EnableCap.ScissorTest);

                GL.Scissor(RenderPosition.X-1, ty, Size.X+3, Size.Y-2);

            }
            else
            {
             //   GL.Disable(EnableCap.ScissorTest);

            }

            foreach (var form in Child)
            {
                //if (form.Position.X > 0 && form.RenderPosition.Y)
               // {
                //    if(form.render)
               
                    form.Render();
                //}
            }
        }

        public virtual bool InBounds(int x,int y)
        {
            if (x >= RenderPosition.X && x <= RenderPosition.X + Size.X && y >= RenderPosition.Y && y <= RenderPosition.Y + Size.Y)
            {
                return true;
            }
            return false;
        }


        public virtual void Resized()
        {


        }

        public virtual void Renamed()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {
            
        }

        public virtual void OnKeyUp(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {

        }

        public virtual void OnKey(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {

        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnLeave()
        {

        }

        public virtual void OnMouseMove(int x,int y,int x_delta,int y_delta)
        {
            
        }

        public virtual void OnMouseDown(int button)
        {
            
        }

        public virtual void OnDoubleClick(int button)
        {
            
        }

        public virtual void OnMouseUp(int button)
        {
            
        }

        public virtual void OnActivate()
        {

        }

        public virtual void OnDeactivate()
        {

        }
        /// <summary>
        /// Drawing
        /// </summary>
        /// 

        public void DrawOutline(Vector4 color)
        {
            
            DrawLine(RenderPosition.X, RenderPosition.Y, RenderPosition.X + Size.X, RenderPosition.Y, color);
            DrawLine(RenderPosition.X, RenderPosition.Y, RenderPosition.X, RenderPosition.Y + Size.Y, color);
            DrawLine(RenderPosition.X, RenderPosition.Y + Size.Y, RenderPosition.X + Size.X, RenderPosition.Y + Size.Y, color);
            DrawLine(RenderPosition.X + Size.X, RenderPosition.Y, RenderPosition.X + Size.X, RenderPosition.Y + Size.Y, color);

        }

        public int TextWidth(string text)
        {
            if (text == "" || text == " ")
            {
                return 0;
            }
            try
            {
                return UserInterface.ActiveInterface.Theme.SystemFont.GenString(text).Width;
            }
            catch(Exception e)
            {
                return 0;
            }


        }

        public int TextHeight(string text)
        {
            if (text == "") return 20;
            return UserInterface.ActiveInterface.Theme.SystemFont.GenString(text).Height;
        }

        public bool Within(int x,int y,int wx,int wy,int ww,int wh)
        {
            return (x >= wx && x <= wx + ww && y >= wy && y <= wy + wh);
        }

        public void DrawText(string text, int x, int y, Vector4 color)
        {
            if (text == null) return;
            if (text == "") return;
            var img = UserInterface.ActiveInterface.Theme.SystemFont.GenString(text);
            Draw(img, x, y, img.Width, img.Height,color);
            

        }

        public void DrawLine(int x,int y,int x2,int y2,Vector4 color)
        {


            if(x == x2)
            {
                Draw(UserInterface.ActiveInterface.Theme.Line, x, y, 1, y2 - y,color);
                return;
            }

            if (y == y2)
            {
                Draw(UserInterface.ActiveInterface.Theme.Line, x, y, x2 - x, 1, color);
                return;

            }

            float xd, yd;

            xd = x2 - x;
            yd = y2 - y;

            float steps = 0;

            if (Math.Abs(xd) > Math.Abs(yd))
            {
                steps = Math.Abs(xd);
            }
            else
            {
                steps = Math.Abs(yd);
            }

            float xi, yi;

            xi = xd / steps;
            yi = yd / steps;

            float dx = x;
            float dy = y;

            for(int i = 0; i < steps; i++)
            {

                Draw(UserInterface.ActiveInterface.Theme.Line, (int)dx, (int)dy, 1, 1, color);

                dx += xi;
                dy += yi;

            }

        }

        public void DrawButton(string text)
        {
            Draw(UserInterface.ActiveInterface.Theme.Button);
            var txt = UserInterface.ActiveInterface.Theme.SystemFont.GenString(text);
            Draw(txt, RenderPosition.X + Size.X / 2 - (txt.Width / 2), RenderPosition.Y + Size.Y / 2 - (txt.Height / 2),txt.Width,txt.Height, new Vector4(1, 1, 1, 1));

        }
        public void DrawFrame(Vector4 color)
        {
            Draw(UserInterface.ActiveInterface.Theme.Frame, color);
        }
        public void DrawFrame(int x,int y,int w,int h,Vector4 color)
        {

            Draw(UserInterface.ActiveInterface.Theme.Frame,x, y, w, h, color);
        }
        public void DrawFrame()
        {
            Draw(UserInterface.ActiveInterface.Theme.Frame);
        }
    
        public void DrawTitle()
        {
            Draw(UserInterface.ActiveInterface.Theme.WindowTitle);
        }

        public void Draw(Texture2D image)
        {
            var render_pos = RenderPosition;
            UserInterface.Draw.Rect(render_pos.X, render_pos.Y, Size.X, Size.Y,image, Color);
        }

        public void Draw(Texture2D image,Vector4 color)
        {
            var render_pos = RenderPosition;
            UserInterface.Draw.Rect(render_pos.X, render_pos.Y, Size.X, Size.Y, image, color);
        }

        public void Draw2(Texture2D image,int x,int y,int w,int h,Vector4 color)
        {
            var render_pos = RenderPosition;
            UserInterface.Draw.Rect(x,y,w,h, image, color);
        }

        public void Draw(Texture2D image, int x,int y,int w,int h,Vector4 col)
        {
            UserInterface.Draw.Rect(x, y, w, h, image, col);
        }
        
    }
    
}
