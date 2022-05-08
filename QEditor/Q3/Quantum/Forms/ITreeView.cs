using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    
    public class TreeItem
    {
        public string Text
        {
            get;
            set;
        }
        
        public Texture.Texture2D Icon
        {
            get;
            set;
        }

        public List<TreeItem> Items
        {
            get;
            set;
        }

        public bool Open
        {
            get;
            set;
        }

        public TreeItem(string text,Texture.Texture2D icon = null)
        {
            Text = text;
            Icon = icon;
            Items = new List<TreeItem>();
            Open = false;
            
        }

        public TreeItem AddItem(string text,Texture.Texture2D icon)
        {
            var item = new TreeItem(text, icon);
            Items.Add(item);
            return item;
        }
        
    }

    public class ITreeView : IActiveContent
    {

        public TreeItem RootItem
        {
            get;
            set;
        }

        public override Vector2i ContentSize
        {
            get
            {
                return new Vector2i(Size.X, max_y);
            }
        }

        public TreeItem OverItem
        {
            get;
            set;
        }

        public TreeItem ActiveItem
        {
            get;
            set;
        }
        int max_y = 0;
        public ITreeView()
        {
            RootItem = new TreeItem("Root Node");
            OverItem = null;
            ActiveItem = null;

        }
       

        public override void RenderForm()
        {
            base.RenderForm();
            DrawFrame(new OpenTK.Mathematics.Vector4(1.5f, 1.5f, 1.5f, 3));

            int startx = RenderPosition.X + 5;
            int starty = RenderPosition.Y + 5;

            max_y = DrawNode(RootItem,startx,starty-ScrollPosition.Y);

            max_y = max_y + ScrollPosition.Y;
            max_y = max_y - RenderPosition.Y;

            max_y = max_y - Size.Y;
            max_y += 30;
            
            Console.WriteLine("scroll:" + ScrollPosition.Y);
            Console.WriteLine("MAXY:" + max_y);

        
        }

        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            //base.OnMouseMove(x, y, x_delta, y_delta);
            int startx = RenderPosition.X + 5;
            int starty = RenderPosition.Y + 5;
            OverItem = null;

            CheckNode(RootItem, startx, starty-ScrollPosition.Y,x,y);

        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if(button==0)
            {
                if (OverItem != null)
                {
                    OverItem.Open = OverItem.Open ? false : true;
                    ActiveItem = OverItem;
                }
            }
        }

        private int CheckNode(TreeItem node, int cx, int cy,int x,int y)
        {
            if(x>=RenderPosition.X && x <= RenderPosition.X + Size.X)
            {
                if (y >= cy && y <= cy + 25)
                {
                    OverItem = node;
                }
                
            }
            if (node.Open)
            {
                foreach (var snode in node.Items)
                {
                    cy = cy + 25;
                    cy = CheckNode(snode,cx + 25, cy, x, y);
                }
            }
            return cy;

        }
        private int DrawNode(TreeItem node,int dx,int dy)
        {

            if(OverItem == node)
            {
                DrawFrame(RenderPosition.X, dy, Size.X, 25, new Vector4(0, 2, 2, 1));
            }else if(ActiveItem == node)
            {
                DrawFrame(RenderPosition.X, dy, Size.X, 25, new Vector4(2, 1, 1, 1));
            }


            if (node.Items.Count > 0)
            {
                DrawOutline(dx, dy + 6, 8, 8, new Vector4(3, 3, 3, 1.0f));

                if (node.Open)
                {


                    DrawFrame(dx, dy + 6, 8, 8, new Vector4(3, 3, 3, 1.0f));


                }

            }
            DrawText(node.Text, dx+16, dy+3, UserInterface.ActiveInterface.Theme.SystemTextColor);


            // dy = dy + 25;
            if (node.Open)
            {
                foreach (var snode in node.Items)
                {
                    dy = dy + 25;
                    dy = DrawNode(snode, dx + 25, dy);



                }
            }
            return dy;

        }

        

        public override void OnResized()
        {
            SetSizeUnsafe(Root.Size);
            //base.OnResized();
        }

    }
    
}
