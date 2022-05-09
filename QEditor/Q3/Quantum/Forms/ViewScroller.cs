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
                float yi, hd, av, ov;
                float nm = 0;
                float ay = 0;
                float max_V = 0;
                yi = hd = av = ov = av2 = 0.0f;
                dh = 0;

                if (Horizontal)
                {
                  
                    ov = (float)(Size.X-(MaxValue)) / (float)(MaxValue);

                    if (ov > 1.0)
                    {
                     
                        ov = 1.0f;
                    }
                    if (ov < 0.1) ov = 0.1f;
      


                    dh = (int)(Size.X * ov);

           

                    if (CurrentValue + dh > Size.X)
                    {
                        if (dh != float.PositiveInfinity)
                        {
                            CurrentValue = Size.X - (int)dh;
                            if (CurrentValue < 0) CurrentValue = 0;
                        }
                    }



                    max_V = Size.X - (dh);
                 
                    av2 = CurrentValue / max_V;
                
                }
                else
                {
                                

                    ov = (float)(Size.Y-MaxValue) / (float)(MaxValue);

                    if (ov > 0.9)
                    {
               
                        ov = 0.9f;
                    }
                    if(ov<0.1)
                    {
                        ov = 0.1f;
                    }
                    




                    dh = (int)(Size.Y * ov);

                  

           


                    if (CurrentValue + dh > Size.Y)
                    {
                        if (dh != float.PositiveInfinity)
                        {
                            CurrentValue = Size.Y - (int)dh;
                            if (CurrentValue < 0) CurrentValue = 0;
                        }
                    }



                    max_V = Size.Y - (dh);
          

                    av2 = CurrentValue / max_V;
              


                }


                return av2;
            }
        }
        float av2;

        private int dh;

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
            if (Horizontal)
            {

                if (msx > RenderPosition.X + CurrentValue && msx < RenderPosition.X + CurrentValue + dh)
                {
                    Drag = true;
                }
                else
                {
                    Drag = false;
                }

            }
            else
            {
                if (msy > RenderPosition.Y + CurrentValue && msy < RenderPosition.Y + CurrentValue + dh)
                {
                    Drag = true;
                }
                else
                {
                    Drag = false;
                }
            }
            //Drag = true;
        }

        public override void OnMouseUp(int button)
        {
            base.OnMouseUp(button);
            Drag = false;
        }
        int msx, msy;
        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            msx = x;
            msy = y;
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
       
       
            DrawFrame();
            DrawOutline(new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            if (Horizontal)
            {
                DrawDragger(RenderPosition.X + CurrentValue, RenderPosition.Y + 1, (int)dh,Size.Y, new OpenTK.Mathematics.Vector4(1.5f,1.5f, 1.5f, 1));
            }
            else
            {
                DrawDragger(RenderPosition.X + 1, RenderPosition.Y + CurrentValue, Size.X - 1, (int)dh, new OpenTK.Mathematics.Vector4(1.5f, 1.5f, 1.5f, 1));
                
            }
        }

    }
}
