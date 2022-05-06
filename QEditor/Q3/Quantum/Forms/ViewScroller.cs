using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public  class ViewScroller : IForm
    {

        public bool Horizontal
        {
            get;
            set;
        }

        public int CurrentValue
        {
            get;
            set;
        }

        public int MaxValue
        {
            get;
            set;
        }

        public float Value
        {
            get
            {
                return av2;
            }
        }
        float av2;


        public ViewScroller()
        {
            Scroll = false;
            //OnMouseDown(0);
            //OnMouseMove(0, 0, 0, 1);
            //OnMouseUp(0);
            CurrentValue = 0;
        }
        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            Drag = true;
        }

        public override void OnMouseUp(int button)
        {
            base.OnMouseUp(button);
            Drag = false;
        }

        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            base.OnMouseMove(x, y, x_delta, y_delta);
            if (Drag)
            {
                if (Horizontal)
                {
                    CurrentValue += x_delta;
                }
                else
                {
                    CurrentValue += y_delta;
                }
                if (CurrentValue < 0) CurrentValue = 0;
                if (Horizontal)
                {
                    if (CurrentValue > Size.X) CurrentValue = Size.X;

                }
                else
                {
                    if (CurrentValue > Size.Y) CurrentValue = Size.Y;
                }
            }
          //  Root.ScrollPosition = new OpenTK.Mathematics.Vector2i(0, (int)av);
            
            // Root.ScrollPosition = new OpenTK.Mathematics.Vector2i(Root.ScrollPosition.X, CurrentValue);
        }

        public override void RenderForm()
        {
            float yi, hd, av, ov, dh;
            float nm = 0;
            float ay = 0;
            float max_V = 0;
            yi = hd = av = ov = av2 = 0.0f;
            dh = 0;

            if (Horizontal)
            {
                yi = (float)(CurrentValue) / (float)(MaxValue);

                hd = Size.X / (float)(MaxValue);

                av = CurrentValue * hd;

                ov = (float)Size.X / (float)(MaxValue);

            
               // Console.WriteLine("HV:" + ov);


                dh = Size.X * ov;

                nm = Size.X - dh;

                ay = CurrentValue;

                if (CurrentValue + dh > Size.X)
                {
                    if (dh != float.PositiveInfinity)
                    {
                        CurrentValue = Size.X - (int)dh;
                        if (CurrentValue < 0) CurrentValue = 0;
                    }
                }



                max_V = Size.X - (dh);
                //float yd = Size.Y - (CurrentValue + dh);
                if (max_V < 1)
                {
                    max_V = 1;
                }
                av2 = CurrentValue / max_V;

                if (av2 > 1)
                {
                    av2 = 1;
                }
            }
            else
            {

                yi = (float)(CurrentValue) / (float)(MaxValue);

                hd = Size.Y / (float)(MaxValue);

//                av = CurrentValue * hd;

                ov = (float)Size.Y / (float)(MaxValue);

               // if (ov > 1) ov = 1;
                

                dh = Size.Y * ov;

                nm = Size.Y - dh;

                ay = CurrentValue;

                if (CurrentValue + dh > Size.Y)
                {
                    if (dh != float.PositiveInfinity)
                    {
                        CurrentValue = Size.Y - (int)dh;
                        if (CurrentValue < 0) CurrentValue = 0;
                    }
                }



                max_V = Size.Y - (dh);
                //float yd = Size.Y - (CurrentValue + dh);
               // Console.WriteLine("AV2aa:" + av2);
                if(max_V<1)
                {
                    max_V = 1;
                }
                av2 = CurrentValue / max_V;
                //Console.WriteLine("CV:" + CurrentValue + " MAX:" + max_V);


                if (av2 == float.NaN || av2 == float.PositiveInfinity || av2==float.NegativeInfinity)
                {
                    av2 = 0;
                }
                if(av2<0)
                {
                    av2 = 0;
                }
                if(av2>1 )
                {
                    av2 = 1;
                }

               
              
               // Console.WriteLine("AV2aa:" + av2);

                if (av2 > 1)
                {
                    av2 = 1;
                }
            }
            //  Console.WriteLine("AV:" + av2);

            //yd = yd / MaxValue;


            //int draw_y = (int)Size.Y 
            //int draw_h = (int)((float)Size.Y * ov);
            skip:
            //base.RenderForm();
            DrawFrame();
            DrawOutline(new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            if (Horizontal)
            {
                DrawFrame(RenderPosition.X + CurrentValue, RenderPosition.Y + 1, (int)dh,Size.Y, new OpenTK.Mathematics.Vector4(1, 3, 3, 1));
            }
            else
            {
                DrawFrame(RenderPosition.X + 1, RenderPosition.Y + CurrentValue, Size.X - 1, (int)dh, new OpenTK.Mathematics.Vector4(1, 3, 3, 1));
                ;
            }
        }

    }
}
