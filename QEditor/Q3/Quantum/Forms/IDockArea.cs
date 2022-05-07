using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum.Forms;
using Q.Quantum;
using Q3.Quantum.Forms;

namespace Q3.Quantum.Forms
{

    public enum DockZone
    {
        Top,Left,Right,Bottom,Center,None
    }
    public class IDockArea : IForm
    {

        public List<IWindow> Left
        {
            get;
            set;
        }

        public List<IWindow> Right
        {
            get;
            set;
        }

        public List<IWindow> Top
        {
            get;
            set;
        }

        public List<IWindow> Bottom
        {
            get;
            set;
        }

        public List<IWindow> Center
        {
            get;
            set;
        }

        public IDockArea()
        {
            Left = new List<IWindow>();
            Right = new List<IWindow>();
            Top = new List<IWindow>();
            Bottom = new List<IWindow>();
            Center = new List<IWindow>();
        }

        public void AddLeft(IWindow win)
        {
            Remove(win);
            Left.Add(win);
        }
        
        public void AddRight(IWindow win)
        {
            Remove(win);
            Right.Add(win);
        }

        public void AddTop(IWindow win)
        {
            Remove(win);
            Top.Add(win);
        }

        public void AddBottom(IWindow win)
        {
            Remove(win);
            Bottom.Add(win);
        }

        public void AddCenter(IWindow win)
        {
            Remove(win);
            Center.Add(win);
        }

        //remove from all
        public void Remove(IWindow win)
        {

            Left.Remove(win);
            Right.Remove(win);
            Top.Remove(win);
            Bottom.Remove(win);
            Center.Remove(win);

        }

        public int LeftCount()
        {
            return Left.Count;
        }

        public int RightCount()
        {
            return Right.Count;
        }

        public int TopCount()
        {
            return Top.Count;
        }

        public int BottomCount()
        {
            return Bottom.Count;
        }

        public int CenterCount()
        {
            return Center.Count;
        }
        public override void RenderForm()
        {
            base.RenderForm();
            DrawFrame();
            DrawOutline(new OpenTK.Mathematics.Vector4(0.7f,0.7f,0.7f,1.0f));
        }

    }
}
