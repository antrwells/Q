using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum;
using Q.Quantum;
using OpenTK.Mathematics;

namespace Q.Quantum.Forms
{
    public class ToolItem
    {
        public string Text
        {
            get;
            set;
        }

        public Q.Texture.Texture2D Icon
        {
            get;
            set;
        }
        
        public Q.Quantum.Forms.IButton Button
        {
            get;
            set;
        }

        public Q.Quantum.Forms.IEnumSelector Enum
        {
            get;
            set;
        }
        
    }
    public class IToolBar : IForm
    {
        public IToolBar()
        {
            Scroll = false;
            if (SeperatorImg == null)
            {
                SeperatorImg = new Texture.Texture2D("Data/UI/Seperator.png");
            }
        }

        int _editx = 5;

        public static Texture.Texture2D SeperatorImg
        {
            get;
            set;
        }


       
        public ToolItem AddItem(Q.Texture.Texture2D icon,string text="")
        {
            ToolItem item = new ToolItem();
            item.Text = text;
            item.Icon = icon;
            item.Button = new Q.Quantum.Forms.IButton();
            item.Button.Set(_editx, 8, 48, 30);
            item.Button.Text = text;
            item.Button.Icon = icon;
            Add(item.Button);
            _editx += 56;
            return item;
        }

        public ToolItem AddEnumSelector(Type type)
        {
            ToolItem item = new ToolItem();

            item.Enum = new Q.Quantum.Forms.IEnumSelector(type);
            item.Enum.Set(_editx, 8, 196, 30);
            Add(item.Enum);
            _editx += 204;
            
            return item;
        }

        public void AddSeperator()
        {

            IImage sep = new IImage();
            sep.SetImage(SeperatorImg);
            sep.Set(_editx, 8, 4, 30);
            _editx += 12;
            Add(sep);

        }

        public void AddSpace(int x)
        {

            _editx += x;
        }

        public override void RenderForm()
        {
            base.RenderForm();
            
            DrawFrame(new Vector4(0.75f, 0.75f,0.75f, 1));
            //DrawOutline(new Vector4(1, 1, 1, 1));
        }
    }
}
