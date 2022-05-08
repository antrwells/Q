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
        
    }
    public class IToolBar : IForm
    {
        public IToolBar()
        {
            Scroll = false;
        }

        int _editx = 5;

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

        public override void RenderForm()
        {
            base.RenderForm();
            
            DrawFrame(new Vector4(0.5f, 0.5f,0.5f, 1));
            //DrawOutline(new Vector4(1, 1, 1, 1));
        }
    }
}
