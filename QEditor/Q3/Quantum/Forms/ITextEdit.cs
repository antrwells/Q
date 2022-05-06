using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{

    public delegate void TextChanged(string text);
    public delegate void NumberChanged(float num);
    public class ITextEdit : IForm
    {


        public event TextChanged OnTextChanged;
        public event NumberChanged OnNumberChanged;
        public bool NumericOnly
        {
            get;
            set;
        }
        private bool claret_on = false;
        private int next_claret = 0;
        private int claret_x;
        private int edit_x = 0;
        private int[] char_x = new int[2500];
        private int start_x = 0;
        public string edit_text = "";
        bool shift = false;

        public string EditText
        {
            get
            {
                return edit_text;
            }
            set
            {
                edit_text = value;
                start_x = 0;
                edit_x = 0;
            }
        }
        public string p_text = "";
        public float p_num = 0.0f;
        public override void RenderForm()
        {
            base.RenderForm();
            DrawFrame();

            if (NumericOnly)
            {
                float num = 0.0f;
                try
                {
                    num = float.Parse(edit_text);
                }
                catch (Exception e)
                {
                    num = 0;
                }
                //finally




                if (num != p_num)
                {
                    p_num = num;
                    OnNumberChanged?.Invoke(num);
                }
            }
            else
            {
                if (edit_text != p_text)
                {
                    p_text = edit_text;
                    OnTextChanged?.Invoke(edit_text);
                }
            }
            //p_text = edit_text;
            
                if (edit_x < start_x)
            {
                start_x = edit_x;
            }
            //   if (!Active)
            //  {
            if (edit_text != "" && edit_text != " ")
            {
                if (edit_x > 0)
                {
                    int ex = char_x[edit_x - 1] - char_x[start_x];
                   // Console.WriteLine("EX:" + ex + "SX:" + start_x);

                    if (ex > Size.X - 25)
                    {
                        start_x++;
                    }
                }

                for (int i = 0; i < edit_text.Length; i++)
                {
                    char_x[i] = TextWidth(edit_text.Substring(0, i + 1));
                }
                if (start_x > 0)
                {
                    string p = edit_text.Substring(start_x);
                    string pt = "";

                    if (p.Length > 0)
                    {
                        for (int i = 0; i <p.Length; i++)
                        {
                            pt = p.Substring(0, i + 1);
                            if (TextWidth(pt) > Size.X - 10)
                            {
                                break;
                            }
                        }


                    }
                 //   Console.WriteLine("P:" + p + " PT:" + pt);
                    DrawText(pt, RenderPosition.X + 5, RenderPosition.Y + 3 + Size.Y / 2 - TextHeight(p) / 2, new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                }
                else
                {

                    string pt = "";

                    if (edit_text.Length >0)
                    {
                        for (int i = 0; i < edit_text.Length; i++)
                        {
                            pt = edit_text.Substring(0, i + 1);
                            if (TextWidth(pt) > Size.X - 10)
                            {
                                break;
                            }
                        }
                        //Console.WriteLine("PT:" + pt);
                    }
                    else
                    {
                        pt = edit_text;
                    }

                    DrawText(pt, RenderPosition.X + 5, RenderPosition.Y + 3 + Size.Y / 2 - TextHeight(edit_text) / 2, new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                }
            }
             DrawOutline(new OpenTK.Mathematics.Vector4(0.6f, 0.6f, 0.6f, 1.0f));

           // edit_x = edit_text.Length;

            if (Active)
            {
                if (claret_on)
                {
                    if (edit_x > 0)
                    {
                        claret_x = char_x[edit_x - 1];
                        if (start_x > 0)
                        {
                            claret_x -= char_x[start_x-1];
                        }
                    }
                    else
                    {
                        claret_x = 0;
                    }
                    if (claret_x < 0)
                    {
                        claret_x = 0;
                    }
                    DrawLine(RenderPosition.X + claret_x + 4, RenderPosition.Y + 4, RenderPosition.X + claret_x + 4, RenderPosition.Y + Size.Y - 3, new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                }
            }
            
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            if (Active)
            {
                int time = Environment.TickCount;
                if(time>next_claret)
                {
                    next_claret = Environment.TickCount + 400;
                    claret_on = claret_on ? false : true;
                }
            }
        }

        public override void OnKeyDown(Keys key)
        {
            base.OnKeyDown(key);
            if(key == Keys.LeftShift || key == Keys.RightShift)
            {
                shift = true;
            }
        }

        public override void OnKeyUp(Keys key)
        {
            base.OnKeyUp(key);
            if(key == Keys.LeftShift || key == Keys.RightShift)
            {
                shift = false;
            }
        }

        public override void OnKey(Keys key)
        {
            //base.OnKey(key);
            //Console.WriteLine(key.ToString());
            AddKey(key);

           
        }

        private void AddKey(Keys key)
        {

            string add_text = "";

            if(key.ToString().Contains("F") && key.ToString().Length > 1)
            {
                return;
            }


            if (key.ToString() == "162")
            {
                if (shift) {
                    add_text = "|";
                }
                else
                {
                    add_text = "\\";
                }
            }
            switch (key)
            {
                case Keys.Enter:
                    return;
                case Keys.LeftControl:
                case Keys.RightControl:
                    return;
                    break;
                case Keys.Left:
                 //   edit_x--;
                    if (edit_x < start_x)
                    {
                        start_x--;
                        edit_x--;
                        if(start_x<0)
                        {
                            start_x = 0;
                        }
                        //Console.WriteLine("EX:" + edit_x + " SX:" + start_x);
                    }
                    else
                    {
                        edit_x--;
                    }
                    
                    if (edit_x < 0) edit_x = 0;
                    return;
                case Keys.Right:
                    edit_x++;
                    if(edit_x>edit_text.Length)
                    {
                        edit_x = edit_text.Length;
                    }
                    return;
                    break;
                case Keys.Delete:

                    if (edit_x < edit_text.Length)
                    {
                        if (edit_x < edit_text.Length)
                        {
                            edit_text = edit_text.Substring(0, edit_x) + edit_text.Substring(edit_x + 1);
                        }
                    }
                    return;
                    break;
                case Keys.Backspace:

                    //edit_text = edit_text.Substring(0, edit_text.Length - 1);
                    if (edit_x == edit_text.Length)
                    {
                        if (edit_x > 0)
                        {
                            edit_text = edit_text.Substring(0, edit_text.Length - 1);
                            edit_x--;
                        }

                    }else if (edit_x > 0)
                    {

                        edit_text = edit_text.Substring(0, edit_x - 1) + edit_text.Substring(edit_x);
                        edit_x--;

                    }

                    return;

                    break;
                case Keys.LeftShift:
                case Keys.RightShift:
                    return;
                    break;
                case Keys.Comma:
                    if (shift)
                    {
                        add_text = "<";
                    }
                    else
                    {
                        add_text = ",";
                    }
                    break;
                case Keys.Period:
                    if (shift)
                    {
                        add_text = ">";
                    }
                    else
                    {
                        add_text = ".";
                    }
                    break;
                case Keys.Minus:
                    if (shift)
                    {
                        add_text = "_";
                    }
                    else
                    {
                        add_text = "-";
                    }
                    break;
                case Keys.Equal:
                    if (shift)
                    {
                        add_text = "+";
                    }
                    else
                    {
                        add_text = "-";
                    }

                    break;
                    
                case Keys.Slash:
                    if (shift)
                    {
                        add_text = "?";
                    }
                    else
                    {
                        add_text = "/";
                    }
                    break;
                case Keys.Space:
                    add_text = " ";
                    break;
                
                case Keys.Backslash:
                    if (shift)
                    {
                        add_text = "\"";
                    }
                    else
                    {
                        add_text = "\\";
                    }
                    break;
                case Keys.Semicolon:
                    if (shift)
                    {
                        add_text = ":";
                    }
                    else
                    {
                        add_text = ";";
                    }
                    break;
                case Keys.Apostrophe:
                    if (shift)
                    {
                        add_text = "\"";
                    }
                    else
                    {
                        add_text = "'";
                    }
                    break;
                case Keys.D0:
                    if (shift)
                    {
                        add_text = ")";
                    }
                    else { 
                        add_text += "0";
                    }
                    break;
                case Keys.D1:
                    if (shift)
                    {
                        add_text = "!";
                    }
                    else
                    {
                        add_text += "1";
                    }
                    break;
                case Keys.D2:
                    if (shift)
                    {
                        add_text = "@";
                    }
                    else
                    {
                        add_text += "2";
                    }
                    break;
                case Keys.D3:
                    if (shift)
                    {
                        add_text = "#";
                    }
                    else
                    {
                        add_text += "3";
                    }
                    break;
                case Keys.D4:
                    if (shift)
                    {
                        add_text = "$";
                    }
                    else
                    {
                        add_text += "4";
                    }
                    break;
                case Keys.D5:
                    if (shift)
                    {
                        add_text = "%";
                    }
                    else
                    {
                        add_text += "5";
                    }
                    break;
                case Keys.D6:
                    if (shift)
                    {
                        add_text = "^";
                    }
                    else
                    {
                        add_text += "6";
                    }
                    break;
                case Keys.D7:
                    if (shift)
                    {
                        add_text = "&";
                    }
                    else
                    {
                        add_text += "7";
                    }
                    break;
                case Keys.D8:
                    if (shift)
                    {
                        add_text = "*";
                    }
                    else
                    {
                        add_text += "8";
                    }
                    break;
                case Keys.D9:
                    if (shift)
                    {
                        add_text = "(";
                    }
                    else
                    {
                        add_text += "9";
                    }
                    break;
                default:
                    add_text = key.ToString();
                    break;
            }

            if (NumericOnly)
            {
                if("0123456789.".Contains(add_text))
                {

                }
                else
                {
                    return;
                }
            }

            if (!shift)
            {
                add_text = add_text.ToLower();
            }

            if (edit_x == edit_text.Length)
            {
                edit_text = edit_text + add_text;

                edit_x++;
            }
            else if (edit_x == 0)
            {
                if (edit_text.Length == 0)
                {
                    edit_text = add_text;
                    edit_x++;
                }
                else
                {
                    edit_text = add_text + edit_text;
                    edit_x++;
                }
            }
            else
            {
                edit_text = edit_text.Substring(0, edit_x) + add_text + edit_text.Substring(edit_x);
                edit_x++;
            }
        }

        public override void OnActivate()
        {
            base.OnActivate();
            
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

    }
}
