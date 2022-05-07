using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum;
namespace Q3.Quantum.Forms
{
    public delegate void ItemSelected(ListItem item);
    public class ListItem
    {
        public string Text { get; set; }
        public Q.Texture.Texture2D Icon { get; set; }
    }
    public class IListSelector : IForm
    {
        public event ItemSelected OnSelected;
        public List<ListItem> Items
        {
            get;
            set;
        }
        
        public ListItem OverItem
        {
            get;
            set;
        }

        public IListSelector()
        {
            Items = new List<ListItem>();
            OverItem = null;
        }

        public ListItem AddItem(string text,Q.Texture.Texture2D icon = null)
        {
            ListItem item = new ListItem();
            item.Text = text;
            item.Icon = icon;
            Items.Add(item);
            Set(Position.X, Position.Y, Size.X, Size.Y + 30);
            return item;
        }

        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            base.OnMouseMove(x, y, x_delta, y_delta);

            int dx = RenderPosition.X + 5;
            int dy = RenderPosition.Y + 5;
            foreach (var item in Items)
            {
                if (x >= dx-5 && x <= dx-5 + Size.X * 10 && y >= dy-4 && y <= dy-4 + 27)
                {
                    OverItem = item;
                }
                    //   DrawText(item.Text, dx, dy, UserInterface.ActiveInterface.Theme.SystemTextColor);
                    dy = dy + 30;
            }
        }
        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            if (OverItem != null)
            {
                OnSelected?.Invoke(OverItem);
            }
        }
        public override void RenderForm()
        {
            base.RenderForm();
            DrawFrame();

            int dx = RenderPosition.X + 5;
            int dy = RenderPosition.Y + 5;
            foreach(var item in Items)
            {
                if (OverItem == item)
                {

                    DrawFrame(dx - 5, dy-4, Size.X, 27, new OpenTK.Mathematics.Vector4(0, 2, 2, 1.0f));
                }
                DrawText(item.Text,dx,dy,UserInterface.ActiveInterface.Theme.SystemTextColor);
                dy = dy + 30;
            }
        }

    }
}
