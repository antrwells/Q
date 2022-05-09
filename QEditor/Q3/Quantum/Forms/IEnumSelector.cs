using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum;

namespace Q.Quantum.Forms
{
    public delegate void EnumSelected(string value);
    public class IEnumSelector : IForm
    {

        public System.Type EType
        {
            get;
            set;
        }

        public List<string> Values = new List<string>();

        public int CurrentSelection
        {
            get;
            set;
        }

        private bool Open
        {
            get;
            set;
        }

        public event EnumSelected OnSelected;

        private IListSelector Selector;

        public  IEnumSelector(Type etype)
        {

            System.Type enumUnderlyingType = System.Enum.GetUnderlyingType(etype);
            System.Array enumValues = System.Enum.GetValues(etype);

            foreach (object enumValue in enumValues)
            {
                Values.Add(enumValue.ToString());
              //  Console.WriteLine("V:" + Values[Values.Count - 1]);
            }
            CurrentSelection = 0;
            Open = false;

        }

        public override void RenderForm()
        {
            base.RenderForm();
            DrawFrameRounded();
            DrawText(Values[CurrentSelection], RenderPosition.X+5, RenderPosition.Y+10, UserInterface.ActiveInterface.Theme.SystemTextColor);
        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (button == 0)
            {

                Open = Open ? false : true;

                if (Open)
                {
                    Selector = new IListSelector();
                    foreach(var v in Values)
                    {
                        Selector.AddItem(v, null);                     
                    }
                    Add(Selector);
                    Selector.Set(0, 35, Size.X, Selector.Size.Y);
                    Selector.OnSelected += Selector_OnSelected;
                }
                else
                {
                    Child.Remove(Selector);
                }

            }
        }

        private void Selector_OnSelected(ListItem item)
        {
            int i = 0;
            foreach(var sel in Values)
            {
                if(sel == item.Text)
                {
                    CurrentSelection = i;
                    Child.Remove(Selector);
                    Open = false;
                    OnSelected?.Invoke(Values[i]);
                    break;
                }
                i++;
            }
        }
    }
}
