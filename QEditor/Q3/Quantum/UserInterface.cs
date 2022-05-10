using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Texture;
using Q.Draw.Simple;
using Q.Quantum.Forms;
using Q.Quantum.Forms;

namespace Q.Quantum
{
    public class UserInterface
    {

        public static UserInterface ActiveInterface
        {
            get;
            set;
        }

        


        public ITheme Theme
        {
            get;
            set;
        }
        private Texture2D Cursor
        {
            get;
            set;
        }
        
        private Q.Quantum.Forms.DockZone HighlightZone
        {
            get;
            set;
        }

        public Q.Quantum.Forms.IWindow DragWin
        {
            get;
            set;
        }

        public Q.Quantum.Forms.IDockArea Docker
        {
            get;
            set;
        }
            

        public IForm FormOver
        {
            get;
            set;
        }

        public IForm[] FormPressed
        {
            get;
            set;
        }

        private long[] PrevClick
        {
            get;
            set;
        }

        public IForm FormActive
        {
            get;
            set;
        }

        public static BasicDraw2D Draw;
                //public static BasicDraw2D DrawBlur;


        public Q.Shader._2D.EXBasicBlur DrawBlur;

        public IForm Root
        {
            get;
            set;
        }

        public IMainMenu MainMenu
        {
            get;
            set;
        }

        public IToolBar ToolBar
        {
            get;
            set;
        }

        bool key_in = false;
        int next_key = 0;
        OpenTK.Windowing.GraphicsLibraryFramework.Keys key;

        private Vector2 prev_mouse;

        public UserInterface()
        {

            Theme = new Themes.ThemeDarkFlat();
            Cursor = new Texture2D("Data/ui/cursor/normal.png", false);
            Draw = new BasicDraw2D();
            DrawBlur = new Shader._2D.EXBasicBlur();
            
            // DrawBlur = new BasicDraw2D();
            Root = new Forms.IGroup();
            Root.Set(0, 70, App.AppInfo.Width, App.AppInfo.Height-70);
            ActiveInterface = this;
            prev_mouse = new Vector2(0, 0);
            FormPressed = new IForm[32];
            PrevClick = new long[32];
            Input.AppInput.OnKeyDown += AppInput_OnKeyDown;
            Input.AppInput.OnKeyUp += AppInput_OnKeyUp;
            Docker = null;
            HighlightZone = Q.Quantum.Forms.DockZone.None;
        }

        public IToolBar AddToolBar()
        {
            ToolBar = new IToolBar();

            if (MainMenu == null)
            {
                ToolBar.Set(0, 0, App.AppInfo.Width, 44);
            }
            else
            {
                ToolBar.Set(0, 25, App.AppInfo.Width, 44);
            }

            return ToolBar;
        }
        private void AppInput_OnKeyUp(OpenTK.Windowing.GraphicsLibraryFramework.Keys obj)
        {
            //throw new NotImplementedException();
            key_in = false;
            if (FormActive != null)
            {
                FormActive.OnKeyUp(obj);
            }
        }

        private void AppInput_OnKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys obj)
        {
            //throw new NotImplementedException();
            //Console.WriteLine("Key:" + obj.ToString());
           // Console.WriteLine(obj.ToString());
            if (FormActive != null)
            {
                FormActive.OnKeyDown(obj);

            }

            if (FormActive != null)
            {
                switch (obj)
                {
                    case OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift:
                    case OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightShift:
                        return;
                        break;
                }

                key_in = true;
                next_key = Environment.TickCount + 400;
                key = obj;
                //FormActive
                FormActive.OnKey(obj);
                
            }

        }

        public IMainMenu AddMainMenu()
        {
            MainMenu = new Forms.IMainMenu();
            MainMenu.Set(0, 0, App.AppInfo.Width,25);
            return MainMenu;
        }
        

        public IForm Add(IForm form)
        {
            Root.Add(form);
            return form;
        }

        public IForm Add(params IForm[] forms)
        {
            foreach(var form in forms)
            {
                Add(form);
            }
            return forms[0];
        }
        int clicks = 0;
        int clicktime = 0;
        bool clicked = false;

        public void DragComplete()
        {
            if (Docker == null) return;
            Docker.Remove(DragWin);

            var zone = HighlightZone;

            MoveTo(zone, DragWin);

            for (int i = 0; i < 1; i++)
            {
                foreach (var win in Docker.Center.ToArray())
                {
                    MoveTo(Q.Quantum.Forms.DockZone.Center, win);
                }
                
                /*
                foreach (var win in Docker.Left.ToArray())
                {
                    MoveTo(Q3.Quantum.Forms.DockZone.Left, win);
                }
                foreach (var win in Docker.Right.ToArray())
                {
                    MoveTo(Q3.Quantum.Forms.DockZone.Right, win);
                }
                foreach (var win in Docker.Top.ToArray())
                {
                    MoveTo(Q3.Quantum.Forms.DockZone.Top, win);
                }
                foreach (var win in Docker.Bottom.ToArray())
                {
                    MoveTo(Q3.Quantum.Forms.DockZone.Bottom, win);
                }
                */
            }







            HighlightZone = Q.Quantum.Forms.DockZone.None;
        }

        private void MoveTo(Q.Quantum.Forms.DockZone zone,IWindow DragWin)
        {
            switch (zone)
            {
                case Q.Quantum.Forms.DockZone.Left:

                    //DragWin.Set(0, 0, Docker.Size.X / 4, Docker.Size.Y);

                    if(Docker.TopCount()==0 && Docker.BottomCount() == 0)
                    {
                        DragWin.Set(0, 0, Docker.Size.X / 4, Docker.Size.Y);
                    }else if (Docker.TopCount() == 0)
                    {

                        DragWin.Set(0,0, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);

                    }else if (Docker.BottomCount() == 0)
                    {
                        DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);
                    }
                    else
                    {
                        DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 2);
                    }


                    Docker.AddLeft(DragWin);

                    break;
                case Q.Quantum.Forms.DockZone.Right:

                    //DragWin.Set(Docker.Size.X - Docker.Size.X / 4, 0, Docker.Size.X / 4, Docker.Size.Y);
                    if (Docker.TopCount() == 0 && Docker.BottomCount() == 0)
                    {
                        DragWin.Set(Docker.Size.X-Docker.Size.X/4, 0, Docker.Size.X / 4, Docker.Size.Y);
                    }
                    else if (Docker.TopCount() == 0)
                    {

                        DragWin.Set(Docker.Size.X-Docker.Size.X/4, 0, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);

                    }
                    else if (Docker.BottomCount() == 0)
                    {
                        DragWin.Set(Docker.Size.X-Docker.Size.X/4, Docker.Size.Y / 4, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);
                    }
                    else
                    {
                        DragWin.Set(Docker.Size.X-Docker.Size.X/4, Docker.Size.Y / 4, Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 2);
                    }


                    Docker.AddRight(DragWin);

                    break;
                case Q.Quantum.Forms.DockZone.Top:

                    //DragWin.Set(0, 0, Docker.Size.X, Docker.Size.Y / 4);

                    if (Docker.LeftCount() == 0 && Docker.RightCount() == 0)
                    {
                        DragWin.Set(0,0, Docker.Size.X, Docker.Size.Y / 4);
                    }
                    else if (Docker.LeftCount() == 0)
                    {
                        DragWin.Set(0,0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y / 4);
                        //DragWin.Set(0, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y / 4);
                    }
                    else if (Docker.RightCount() == 0)
                    {

                        DragWin.Set(Docker.Size.X / 4, 0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y / 4);

                    }
                    else
                    {
                        DragWin.Set(Docker.Size.X / 4,0, Docker.Size.X / 2, Docker.Size.Y / 4);
                    }


                    Docker.AddTop(DragWin);


                    break;
                case Q.Quantum.Forms.DockZone.Bottom

                :
                    if (Docker.LeftCount() == 0 && Docker.RightCount() == 0)
                    {
                        DragWin.Set(0, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X, Docker.Size.Y / 4);
                    }
                    else if (Docker.LeftCount() == 0)
                    {
                        DragWin.Set(0, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y / 4);
                        //DragWin.Set(0, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y / 4);
                    }
                    else if (Docker.RightCount() == 0)
                    {
                        //if (Docker.Left[0].Size.Y >= Docker.Size.Y-2)
                        {

                            DragWin.Set(Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y / 4);
                        }
                      //  else
                        {
                          //  DragWin.Set(0, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X, Docker.Size.Y / 4);
                        }
                    }
                    else
                    {
                        DragWin.Set(Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y / 4);
                    }

                    Docker.AddBottom(DragWin);


                    break;

                case Q.Quantum.Forms.DockZone.Center:


                    if (Docker.LeftCount() == 0 && Docker.RightCount() == 0)
                    {
                        if (Docker.TopCount() == 0 && Docker.BottomCount() == 0)
                        {
                            //Docker.AddCenter(DragWin);
                            DragWin.Set(0, 0, Docker.Size.X, Docker.Size.Y);
                        }
                        else if (Docker.TopCount() == 0)
                        {
                            DragWin.Set(0, 0, Docker.Size.X, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else if (Docker.BottomCount() == 0)
                        {
                            DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else
                        {
                            DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X, Docker.Size.Y - Docker.Size.Y / 2);
                        }


                    }
                    else if (Docker.LeftCount() == 0)
                    {

                        if (Docker.TopCount() == 0 && Docker.BottomCount() == 0)
                        {
                            //Docker.AddCenter(DragWin);
                            DragWin.Set(0, 0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y);
                        }
                        else if (Docker.TopCount() == 0)
                        {
                            DragWin.Set(0, 0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else if (Docker.BottomCount() == 0)
                        {
                            DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y);
                        }
                        else
                        {
                            DragWin.Set(0, Docker.Size.Y / 4, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y / 2);
                        }

                    }
                    else if (Docker.RightCount() == 0)
                    {
                        if (Docker.TopCount() == 0 && Docker.BottomCount() == 0)
                        {
                            //Docker.AddCenter(DragWin);
                            DragWin.Set(Docker.Size.X / 4, 0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y);
                        }
                        else if (Docker.TopCount() == 0)
                        {
                            DragWin.Set(Docker.Size.X / 4, 0, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else if (Docker.BottomCount() == 0)
                        {
                            DragWin.Set(Docker.Size.X / 4, Docker.Size.Y / 4, Docker.Size.X - Docker.Size.X / 4, Docker.Size.Y);
                        }

                    }
                    else
                    {
                        if (Docker.TopCount() == 0 && Docker.BottomCount() == 0)
                        {
                            DragWin.Set(Docker.Size.X / 4, 0, Docker.Size.X / 2, Docker.Size.Y);
                        }
                        else if (Docker.TopCount() == 0)
                        {
                            DragWin.Set(Docker.Size.X / 4, 0, Docker.Size.X / 2, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else if (Docker.BottomCount() == 0)
                        {
                            DragWin.Set(Docker.Size.X / 4, Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y - Docker.Size.Y / 4);
                        }
                        else
                        {
                            DragWin.Set(Docker.Size.X / 4, Docker.Size.Y / 4, Docker.Size.X / 4, Docker.Size.Y / 4);
                        }
                    }
                    //DragWin.Set(Docker.Size.X / 4, Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y / 2);
                    Docker.AddCenter(DragWin);



                    break;
            }
        }

        public void UpdateUI()
        {

            if (Docker != null)
            {
                if (DragWin != null)
                {
                    var dpos = DragWin.RenderPosition;

                    HighlightZone = Q.Quantum.Forms.DockZone.None;

                    //left
                    if (dpos.X > Docker.RenderPosition.X && dpos.X < Docker.RenderPosition.X + Docker.Size.X / 4)
                    {
                        if (dpos.Y > Docker.RenderPosition.Y && dpos.Y < Docker.RenderPosition.Y + Docker.Size.Y)
                        {

                            HighlightZone = Q.Quantum.Forms.DockZone.Left;

                        }

                    }

                    if (dpos.X >= Docker.RenderPosition.X + Docker.Size.X - Docker.Size.X / 4 && dpos.X <= Docker.RenderPosition.X + Docker.Size.X)
                    {
                        if (dpos.Y > Docker.RenderPosition.Y && dpos.Y < Docker.RenderPosition.Y + Docker.Size.Y)
                        {

                            HighlightZone = Q.Quantum.Forms.DockZone.Right;

                        }
                    }

                    if (dpos.Y >= Docker.RenderPosition.Y && dpos.Y <= Docker.RenderPosition.Y + Docker.Size.Y / 4)
                    {

                        if (dpos.X >= Docker.RenderPosition.X && dpos.X <= Docker.RenderPosition.X + Docker.Size.X)
                        {

                            HighlightZone = Q.Quantum.Forms.DockZone.Top;

                        }

                    }

                    if (dpos.Y >= (Docker.RenderPosition.Y + Docker.Size.Y) - Docker.Size.Y / 4 && dpos.Y <= Docker.RenderPosition.Y + Docker.Size.Y)
                    {

                        if (dpos.X >= Docker.RenderPosition.X && dpos.X <= Docker.RenderPosition.X + Docker.Size.X)
                        {

                            HighlightZone = Q.Quantum.Forms.DockZone.Bottom;

                        }

                    }

                    if (dpos.X >= Docker.Position.X + Docker.Size.X / 4 && dpos.X <= Docker.Position.X + Docker.Size.X - Docker.Size.X / 4)
                    {
                        if (dpos.Y >= Docker.Position.Y + Docker.Size.Y / 4 && dpos.Y <= Docker.Position.Y + Docker.Size.Y - Docker.Size.Y / 4)
                        {
                            HighlightZone = Q.Quantum.Forms.DockZone.Center;
                        }

                    }
                }
            }

            if (key_in)
            {

                if (FormActive != null)
                {
                    if (Environment.TickCount > next_key)
                    {
                        FormActive.OnKey(key);
                        next_key = next_key + 150;
                    }
                }
            }

            Vector2 cur_mouse = Input.AppInput.MousePosition;

            List<IForm> forms = new List<IForm>();

            AddForms(forms, Root);
            if (ToolBar != null)
            {
                AddForms(forms, ToolBar);
            }
            if (MainMenu != null)
            {
                AddForms(forms, MainMenu);
            }
            
            forms.Reverse();

            foreach(var form in forms)
            {
                form.OnUpdate();
            }


            var form_over = GetFormOver(forms, (int)Input.AppInput.MousePosition.X, (int)Input.AppInput.MousePosition.Y);
            
            

            if (form_over != FormOver && form_over !=null)
            {

                if (FormPressed[0] != form_over)
                {
                    form_over.OnEnter();
                }

                if (FormOver != null)
                {
                    if (FormPressed[0] != FormOver)
                    {
                        FormOver.OnLeave();
                    }
                }
                if(FormOver!=form_over && FormOver!=null)
                {
                    if (FormOver == FormPressed[0])
                    {
                        if (Input.AppInput.MouseButton[0] == false)
                        {
                            FormOver.OnMouseUp(0);
                            FormPressed[0] = null;
                        }
                    }
                }
                FormOver = form_over;

            }
            else
            {

                

            }


            //check for double click
            

            if (Input.AppInput.MouseButton[0])
            {
                if (!clicked)
                {
                    clicked = true;
                    clicktime = Environment.TickCount;
                }
            }
            else
            {
                if (clicks > 0)
                {
                    int tt = Environment.TickCount - clicktime;
                    if (tt > 250)
                    {
                        clicks = 0;
                        clicked = false;
                    }
                }
                if (clicked)
                {
                   

                    
                    clicked = false;
                    int tt = Environment.TickCount - clicktime;
                    if (tt < 250)
                    {
                        clicks++;
                    }
                    else
                    {
                        clicks = 0;
                    }
                    //clicktime = Environment.TickCount;
                    if (clicks == 2)
                    {
                        if (form_over != null)
                        {
                            form_over.OnDoubleClick(0);
                            clicks = 0;
                        }
                    }
                    
                }
            }

              
            

            for (int i = 0; i < 16; i++)
            {
                if (Input.AppInput.MouseButton[i])
                {
                    if (i == 0)
                    {
                      
                    }                    
                    if (FormPressed[i] == null && form_over!=null)
                    {
                        form_over.OnMouseDown(i);
                        FormPressed[i] = form_over;
                        if(FormActive!=null && FormActive != form_over)
                        {
                            FormActive.OnDeactivate();
                            FormActive.Active = false;
                        }
                        FormActive = form_over;
                        form_over.OnActivate();
                        form_over.Active = true;
                        
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (i == 0)
                    {
                       
                    }
                    if (FormPressed[i] !=null)
                    {
                        FormPressed[i].OnMouseUp(i);
                        FormPressed[i] = null;

                    }
                }

            }

            if (FormPressed[0] != null)
            {
                form_over = FormPressed[0];
            }
            else if (FormOver != null)
            {
                form_over = FormOver;
            }

            if (form_over != null)
            {
                if (prev_mouse != cur_mouse)
                {
                    Vector2 delta = cur_mouse - prev_mouse;
                    form_over.OnMouseMove((int)cur_mouse.X, (int)cur_mouse.Y, (int)delta.X, (int)delta.Y);

                }
            }

            prev_mouse = Input.AppInput.MousePosition;
        }

        private IForm GetFormOver(List<IForm> forms,int x,int y)
        {

            foreach (var form in forms)
            {
                if (form.InBounds(x,y))
                {
                    return form;
                }
            }
            return null;

        }


        private void AddForms(List<IForm> forms, IForm form)
        {
            forms.Add(form);
            foreach (var f in form.Child)
            {
                AddForms(forms, f);
            }   
        }

        public void RenderUI()
        {
            Root.Render();

            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);

            switch (HighlightZone)
            {
                case Q.Quantum.Forms.DockZone.Left:

                    Draw.Rect(Docker.RenderPosition.X, Docker.RenderPosition.Y, Docker.Size.X / 4, Docker.Size.Y, Theme.Frame, new Vector4(1, 1.5f, 1.5f, 0.8f));
                    
                    break;
                case Q.Quantum.Forms.DockZone.Right:

                    Draw.Rect(Docker.RenderPosition.X + Docker.Size.X - Docker.Size.X / 4, Docker.RenderPosition.Y, Docker.Size.X / 4, Docker.Size.Y, Theme.Frame, new Vector4(1, 1.5f,1.5f, 0.8f));

                    break;
                case Q.Quantum.Forms.DockZone.Top:

                    Draw.Rect(Docker.RenderPosition.X, Docker.RenderPosition.Y, Docker.Size.X, Docker.Size.Y / 4, Theme.Frame, new Vector4(1, 1.5f,1.5f, 0.8f));

                    break;
                case Q.Quantum.Forms.DockZone.Bottom:

                    Draw.Rect(Docker.RenderPosition.X, Docker.RenderPosition.Y + Docker.Size.Y - Docker.Size.Y / 4, Docker.Size.X, Docker.Size.Y / 4, Theme.Frame, new Vector4(1, 1.5f,1.5f, 0.8f));
                    break;
                case Q.Quantum.Forms.DockZone.Center:

                    Draw.Rect(Docker.RenderPosition.X + Docker.Size.X / 4, Docker.RenderPosition.Y + Docker.Size.Y / 4, Docker.Size.X / 2, Docker.Size.Y / 2, Theme.Frame, new Vector4(1, 1.5f,1.5f, 0.8f));

                                 break;
                    
                    

            }
            
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);
           
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);
            if (ToolBar != null)
            {
                ToolBar.Render();
            }
            if (MainMenu != null)
            {
                MainMenu.Render();
            }

            RenderCursor();

        }

        private void RenderCursor()
        {
            Draw.Rect((int)Input.AppInput.MousePosition.X, (int)Input.AppInput.MousePosition.Y, 32, 32, Cursor, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }
    }
}
