using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Q.Texture;
using Q.Draw.Simple;
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

        bool key_in = false;
        int next_key = 0;
        OpenTK.Windowing.GraphicsLibraryFramework.Keys key;

        private Vector2 prev_mouse;

        public UserInterface()
        {

            Theme = new Themes.ThemeDarkFlat();
            Cursor = new Texture2D("Data/ui/cursor/normal.png", false);
            Draw = new BasicDraw2D();
            Root = new Forms.IGroup();
            Root.Set(0, 25, App.AppInfo.Width, App.AppInfo.Height-25);
            ActiveInterface = this;
            prev_mouse = new Vector2(0, 0);
            FormPressed = new IForm[32];
            PrevClick = new long[32];
            Input.AppInput.OnKeyDown += AppInput_OnKeyDown;
            Input.AppInput.OnKeyUp += AppInput_OnKeyUp;
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
        public void UpdateUI()
        {

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
                        form_over.OnDoubleClick(0);
                        clicks = 0;
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
                    if (FormPressed[i] == null)
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
            if (MainMenu != null)
            {
                MainMenu.Render();
            }
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);

            RenderCursor();

        }

        private void RenderCursor()
        {
            Draw.Rect((int)Input.AppInput.MousePosition.X, (int)Input.AppInput.MousePosition.Y, 32, 32, Cursor, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }
    }
}
