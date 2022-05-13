using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Texture;
using OpenTK.Graphics.OpenGL;


/// <summary>
/// Q.Quantum is a custom GUI for Q that is easy to extend and comes with a rich
/// set of already buillt forms.
/// </summary>
namespace Q.Quantum
{

    /// <summary>
    /// The baseline class for any form. Always inherit from this if adding your own elements.
    /// </summary>
    public class IForm
    {

        /// <summary>
        /// The parent form of any form.
        /// </summary>
        public IForm Root
        {
            get;
            set;
        }

        /// <summary>
        /// A list of child forms.
        /// </summary>
        public List<IForm> Child
        {
            get;
            set;
        }

        /// <summary>
        /// The local position of a form in pixels.
        /// </summary>
        public Vector2i Position
        {
            get;
            set;
        }

        /// <summary>
        /// If true, a form will not generate any InBound calls, making it purely cosmetic.
        /// </summary>
        public bool NoInteract
        {
            get;
            set;
        }


        /// <summary>
        /// The size of a form in pixels.
        /// </summary>
        public Vector2i Size
        {
            get
            {
                return _Size;
            }
            set
            {
                var ps = _Size;
                _Size = value;
                if (ps != _Size)
                {
                    //   OnSizeChanged();
                    Resized();
                }
            }
        }
        private Vector2i _Size = new Vector2i(0, 0);

        /// <summary>
        /// The color of a form, used in different ways for different forms.
        /// </summary>
        public Vector4 Color
        {
            get;
            set;
        }

        /// <summary>
        /// If true, a formo is rendered and visible, if false, then it is not.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// If true, a form is active and reciving input.
        /// </summary>
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

        /// <summary>
        /// If the mouse is hovering over the form, this property will be set to true, false if not.
        /// </summary>
        public bool Over
        {
            get;
            set;
        }

        /// <summary>
        /// Is true if the form is being dragged by the user.
        /// </summary>
        public bool Drag
        {
            get;
            set;
        }

        public bool AcceptDrops
        {
            get;
            set;
        }

        /// <summary>
        /// The main text of a form. For a button this would be it's label text, for example.
        /// </summary>
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

        /// <summary>
        /// The scroll position. This alters all child forms in some forms, not all.
        /// </summary>
        public Vector2i ScrollPosition
        {
            get;
            set;
        }

        public bool CanDragAndDrop
        {
            get;
            set;
        }

        /// <summary>
        /// If true, this form can be scrolled by it's parent form.
        /// </summary>
        public bool Scroll
        {
            get;
            set;
        }


        /// <summary>
        /// The content size of a form. This can be overriden to allow extended functionality.
        /// </summary>
        public virtual Vector2i ContentSize
        {

            get
            {

                int bx = 0;
                int by = 0;

                if (Child.Count > 0)
                {

                    foreach (var form in Child)
                    {
                        var x = form.Position.X + (form.Size.X - Size.X);
                        var y = form.Position.Y + (form.Size.Y - Size.Y) + 25;

                        if (x < 0) x = 0;
                        if (y < 0) y = 0;

                        // x = x - Size.X/2;
                        //y = y - Size.Y;

                        //                    x = form.Position.X 


                        if (x > bx) bx = x;
                        if (y > by) by = y;
                    }
                }
                else
                {
                    
                }

               
                return new Vector2i(bx, by);

            }
            
        }


        /// <summary>
        /// This generates the final render position of a form, in pixels.
        /// </summary>
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

        /// <summary>
        /// If true, this form can generate it's child forms scrolling based on input.
        /// </summary>
        public bool ChildScroll
        {
            get;
            set;
        }
       
        public Vector4 ScissorOffset
        {
            get;
            set;
                
        }

        public bool CheckBounds
        {
            get;
                set;
        }

        public virtual void CompleteDrop(DragInfo info)
        {
            
        }
        /// <summary>
        /// The constructor, this sets up the form for basic use.
        /// </summary>
        public IForm()
        {
            AcceptDrops = false;
            Root = null;
            Child = new List<IForm>();
            Scroll = true;
            ChildScroll = false;
           // Set(0, 0, 0, 0);
            SetText("");
            SetColor(1, 1, 1, 1);
            NoInteract = false;
            CheckBounds = false;
            ScissorOffset = new Vector4();

        }

        public virtual DragInfo GetDragInfo()
        {


            return null;
            
        }

        /// <summary>
        /// Lets you set the color property of the form.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public IForm SetColor(float r,float g,float b,float a)
        {
            SetColor(new Vector4(r, g, b, a));
            return this;
        }

        /// <summary>
        /// Lets you see the color property of the form.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public IForm SetColor(Vector4 color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// This is the primary way of setting up a forms position and size.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public IForm Set(int x, int y, int w, int h)
        {
            Position = new Vector2i(x, y);
            Size = new Vector2i(w, h);
            //Resized();
            return this;
        }

        /// <summary>
        /// Allows you to set the primary text of a form.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IForm SetText(string text)
        {
            Text = text;
            Renamed();
            return this;
        }

        /// <summary>
        /// Allows you to add a list of forms in a single call.
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public IForm Add(params IForm[] forms)
        {
            foreach (var form in forms)
            {
                Add(form);
            }
            return forms[0];
        }

        /// <summary>
        /// Allows you to change the size of a form, without it generating resized methods.
        /// </summary>
        /// <param name="size"></param>
        public void SetSizeUnsafe(Vector2i size)
        {
            _Size = size;
        }

        /// <summary>
        /// Lets you add a child form to this form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public IForm Add(IForm form)
        {

            form.Root = this;
            Child.Add(form);
            form.Resized();
            return this;
        }

        /// <summary>
        /// Renders a form. This is an internal method.
        /// </summary>
        public void Render()
        {

            PreRender();
            RenderForm();
            PostRender();
            RenderChildren();

        }


        public virtual void PreRender()
        {

        }
        
        public virtual void PostRender()
        {
            
        }

        /// <summary>
        /// This is the method to override to render your own custom forms.
        /// </summary>
        public virtual void RenderForm()
        {

        }

        /// <summary>
        /// Internal method.
        /// </summary>
        public void RenderChildren()
        {
            //GL.Viewport(RenderPosition.X, RenderPosition.Y, Size.X, Size.Y);
            //GL.Viewport(0, RenderPosition.Y-Size.Y, App.AppInfo.Width, Size.Y);



            if (this is Forms.IGroup)
            {
                int ty = App.AppInfo.Height - (RenderPosition.Y + Size.Y);

                int sy = App.AppInfo.Height - ty;

                GL.Enable(EnableCap.ScissorTest);

                GL.Scissor(RenderPosition.X - 3 + (int)ScissorOffset.X, ty  + (int)ScissorOffset.Y, Size.X + 3 + (int)ScissorOffset.Z,(Size.Y -3) + (int)ScissorOffset.W);

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
                if (form.RenderPosition.Y < RenderPosition.Y)
                {
                    //   form.NoInteract = true;
                }
                else
                {
                    //    form.NoInteract = false;
                }
                form.Render();
                //}
            }
            if (this is Forms.IGroup)
            {
                GL.Disable(EnableCap.ScissorTest);
            }
        }
        
        /// <summary>
        /// returns true if the coords (x,y) are within bounds of a form.
        /// This can be overriden for more complex behaviour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool InBounds(int x,int y)
        {
            if (NoInteract)
            {
                return false;
            }
            if (x >= RenderPosition.X && x <= RenderPosition.X + Size.X && y >= RenderPosition.Y && y <= RenderPosition.Y + Size.Y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is called whenever a form is resized. You can override this method
        /// for your own logic when resized.
        /// </summary>
        public virtual void OnResized()
        {


        }

        public void Resized()
        {
            OnResized();
            foreach(var form in Child)
            {
               // form.Resized();
            }
        }

        /// <summary>
        /// Called if a form is renamed.
        /// </summary>
        public virtual void Renamed()
        {
            
        }

        /// <summary>
        /// Called once per frame, you override this method to provide your own update
        /// logic for your own custom forms.
        /// </summary>
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

        /// <summary>
        /// This method is called when the mouse enters a form's area.
        /// You can override this for your own logic.
        /// </summary>
        public virtual void OnEnter()
        {

        }

        /// <summary>
        /// This method is called when the mouse leaves a form's area.
        /// </summary>
        public virtual void OnLeave()
        {

        }

        /// <summary>
        /// This is called when the mouse is moving across a form's surface.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x_delta"></param>
        /// <param name="y_delta"></param>
        public virtual void OnMouseMove(int x,int y,int x_delta,int y_delta)
        {
            
        }

        /// <summary>
        /// Called when a mouse down event happens when the mouse is over a form.
        /// </summary>
        /// <param name="button"></param>
        public virtual void OnMouseDown(int button)
        {
            
        }

        /// <summary>
        /// Called if the mouse is double clicked when over a form.
        /// </summary>
        /// <param name="button"></param>
        public virtual void OnDoubleClick(int button)
        {
            
        }

        /// <summary>
        /// Called when a mouse up event happens when the mouse is over a form.
        /// </summary>
        /// <param name="button"></param>
        public virtual void OnMouseUp(int button)
        {
            
        }

        /// <summary>
        /// Called when a form is activated.
        /// </summary>
        public virtual void OnActivate()
        {

        }

        /// <summary>
        /// Called when a form is deactivated.
        /// </summary>
        public virtual void OnDeactivate()
        {

        }
        /// <summary>
        /// Drawing
        /// </summary>
        /// 

        public void DrawOutline(int x,int y,int w,int h,Vector4 color)
        {

            DrawLine(x,y, x+w, y, color);
            DrawLine(x,y, x, y+h, color);
            DrawLine(x,y+h,x+w, y+h, color);
            DrawLine(x+w, y, x+w,y+h, color);
            

        }

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

        public void DrawButtonNoText()
        {
            Draw(UserInterface.ActiveInterface.Theme.Button);
        }

        public void DrawButton(string text)
        {
            Draw(UserInterface.ActiveInterface.Theme.Button);
            var txt = UserInterface.ActiveInterface.Theme.SystemFont.GenString(text);
            if (txt != null)
            {
                Draw(txt, RenderPosition.X + Size.X / 2 - (txt.Width / 2), RenderPosition.Y + Size.Y / 2 - (txt.Height / 2), txt.Width, txt.Height, new Vector4(1, 1, 1, 1));

            }                
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

        public void DrawDragger()
        {
            Draw(UserInterface.ActiveInterface.Theme.Dragger);
        }

        public void DrawDragger(int x, int y, int w, int h, Vector4 color)
        {
            Draw(UserInterface.ActiveInterface.Theme.Dragger, x, y, w, h, color);
        }
        public void DrawFrameRounded()
        {
            Draw(UserInterface.ActiveInterface.Theme.FrameRounded);
        }

        public void DrawFrameRounded(Vector4 color)
        {
            Draw(UserInterface.ActiveInterface.Theme.FrameRounded, color);

        }
        Texture2D bg = null;
        public void DrawBlur(int x,int y,int w,int h,float blur = 0.5f)
        {

            if (bg == null)
            {
                bg = new Texture2D(w, h);
            }
            else
            {
                if(bg.Width!=w || bg.Height!=h)
                {
                    bg.DestroyNow = true;
                    bg.DestroyTexture();
                    bg = new Texture2D(w, h);
                }
            }
            int ty = App.AppInfo.Height - (y + h);

            int sy = App.AppInfo.Height - ty;
                      

                        bg.CopyTex(x, ty);


            UserInterface.Draw.Rect(UserInterface.ActiveInterface.DrawBlur,x, y+h, w, -h, bg, new Vector4(1,1,1, 1.0f)) ;

            //Console.WriteLine("B:" + x + " Y:" + y + " W:" + w + " H:" + h);

        }
        
        public void DrawFrameRounded(int x, int y, int w, int h, Vector4 color)
        {

            Draw(UserInterface.ActiveInterface.Theme.FrameRounded, x, y, w, h, color);

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
