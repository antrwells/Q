﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Quantum.Forms
{
    public delegate void MenuClick(MenuItem item);
    public class MenuItem
    {
        public string Text = "";
        public bool Open = false;
        public List<MenuItem> Items = new List<MenuItem>();
        public int DX = 0;
        public Texture.Texture2D Icon = null;
        public MenuClick CLick = null;
        public MenuItem AddItem(string text)
        {
            MenuItem new_item = new MenuItem();
            new_item.Text = text;
            Items.Add(new_item);
            return new_item;
        }
        public ISubMenu SubMenu = null;
    }

    public class ISubMenu : IForm
    {
        public List<MenuItem> Items = new List<MenuItem>();
        public MenuItem OverItem = null;
        public MenuItem AddItem(string text)
        {
            var new_item = new MenuItem();
            new_item.Text = text;
            Items.Add(new_item);
            return new_item;
        }
        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            base.OnMouseMove(x, y, x_delta, y_delta);
            int dx = RenderPosition.X + 3;
            int dy = RenderPosition.Y + 6;

            foreach (var item in Items)
            {

              //  DrawText(item.Text, dx, dy, new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                if(x>=dx && x<=dx+Size.X && y>=dy && y <= dy + 25)
                {
                    OverItem = item;
                }

                dy = dy + 25;

            }
        }

        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            if (OverItem!=null)
            {
                OverItem.CLick?.Invoke(OverItem);
                if(OverItem.Open == true)
                {
                    Child.Remove(OverItem.SubMenu);
                    OverItem.Open = false;
                }else if(OverItem.Open == false)
                {
                    OverItem.Open = true;
                    if (OverItem.SubMenu != null)
                    {

                        //var sub = MenuItems[OverItem];
                        Add(OverItem.SubMenu);
                    }
                    else
                    {
                        if (OverItem.Items.Count == 0)
                        {
                            return;
                        }
                        var new_sub = new ISubMenu();
                        int need_y = OverItem.Items.Count * 25 + 10;

                        int s_w = 10;
                        int ay = 0;

                        foreach (var ci in OverItem.Items)
                        {

                            int aw = TextWidth(ci.Text) + 25;
                            if (aw > s_w)
                            {
                                s_w = aw;
                            }

                            var new_subi = new_sub.AddItem(ci.Text);
                            new_subi.Items = ci.Items;
                            new_subi.CLick = ci.CLick;
                            new_subi.Icon = ci.Icon;
                        }

                        new_sub.Set(Size.X+3, ay, s_w+150, need_y);
                        OverItem.SubMenu = new_sub;
                        ay = ay + 25;
                        Console.WriteLine("Item:" + OverItem.Text);
                        Add(new_sub);
                        //Environment.Exit(0);



                    }
                }

            }    
        }


        public override void RenderForm()
        {
            //base.RenderForm();

            DrawFrame(new Vector4(1.1f, 1.1f, 1.1f, 1.0f));
            DrawLine(RenderPosition.X, RenderPosition.Y, RenderPosition.X + Size.X, RenderPosition.Y,new Vector4(0.3f,0.3f,0.3f,1.0f));
            DrawLine(RenderPosition.X, RenderPosition.Y, RenderPosition.X , RenderPosition.Y+Size.Y, new Vector4(0.3f, 0.3f, 0.3f, 1.0f));
            DrawLine(RenderPosition.X+Size.X, RenderPosition.Y, RenderPosition.X + Size.X, RenderPosition.Y+Size.Y, new Vector4(0.3f, 0.3f, 0.3f, 1.0f));
            DrawLine(RenderPosition.X , RenderPosition.Y+Size.Y, RenderPosition.X + Size.X, RenderPosition.Y + Size.Y, new Vector4(0.3f, 0.3f, 0.3f, 1.0f));

            int dx = RenderPosition.X + 3;
            int dy = RenderPosition.Y + 6;

            foreach(var item in Items)
            {
                if (item == OverItem)
                {
                    DrawFrame(dx,dy-3,Size.X-4,25,new Vector4(0.2f,2.4f,2.4f,1.0f));
                }
                if(item.Icon!=null)
                {
                    Draw(item.Icon, dx, dy+1, 16, 16,new Vector4(1,1,1,1));
                }
                DrawText(item.Text, dx+32, dy, new Vector4(0.7f, 0.7f, 0.7f, 1.0f));
                if (item.Items.Count > 0)
                {
                    Draw(IMainMenu.RightArrow, RenderPosition.X+ Size.X - 32, dy,16, 16, new Vector4(1, 1, 1, 1));
                }
                
                dx = dx;
                dy = dy + 25;

            }

        }

    }

    public class IMainMenu : IForm
    {

        public List<MenuItem> Items = new List<MenuItem>();
        public static Texture.Texture2D RightArrow = null;
        
        public MenuItem OverItem
        {
            get;
            set;
        }

        public Dictionary<MenuItem, ISubMenu> MenuItems = new Dictionary<MenuItem, ISubMenu>();
        public IMainMenu()
        {
            if (RightArrow == null)
            {
                RightArrow = new Texture.Texture2D("Data/UI/right.png", false);
            }
        }




        public override void RenderForm()
        {
            DrawFrame(new Vector4(1.1f, 1.1f, 1.1f, 1.0f));
            int dx = RenderPosition.X + 25;
            int dy = RenderPosition.Y + 6;
            foreach(var item in Items)
            {
                if (OverItem == item)
                {
                    DrawFrame(dx-25,dy-6,TextWidth(item.Text)+80,Size.Y, new Vector4(0.5f, 0.5f, 0.5f, 1));
                }
                DrawText(item.Text, dx+3, dy+1, new Vector4(0.7f,0.7f, 0.7f, 1));
                item.DX = dx;
                dx += TextWidth(item.Text) + 60;
            }
            //base.RenderForm();
        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (OverItem != null)
            {
                if (OverItem.Open)
                {
                    OverItem.Open = false;
                    //if (MenuItems.ContainsKey(OverItem))
                    
                        Child.Remove(OverItem.SubMenu);
                    
                }
                else
                {
                    OverItem.Open = true;
                    if (OverItem.SubMenu!=null)
                    {

                        //var sub = MenuItems[OverItem];
                        Add(OverItem.SubMenu);
                    }
                    else
                    {
                        if (OverItem.Items.Count == 0) return;
                        var new_sub = new ISubMenu();
                        int need_y = OverItem.Items.Count * 25 + 10;

                        int s_w = 10;

                        foreach(var ci in OverItem.Items)
                        {

                            int aw = TextWidth(ci.Text) + 25;
                            if (aw > s_w)
                            {
                                s_w = aw;
                            }
                           
                            var new_subi = new_sub.AddItem(ci.Text);
                            new_subi.Items = ci.Items;
                            new_subi.CLick = ci.CLick;
                            new_subi.Icon = ci.Icon;
                        }
                      
                        new_sub.Set(OverItem.DX-10, 30, s_w+150, need_y);
                        OverItem.SubMenu = new_sub;
                        Add(new_sub);
                        //Environment.Exit(0);

                        

                    }
                }
            }
        }
        public override void OnMouseMove(int x, int y, int x_delta, int y_delta)
        {
            //base.OnMouseMove(x, y, x_delta, y_delta);
            int dx = RenderPosition.X + 25;
            int dy = RenderPosition.Y + 6;
            foreach (var item in Items)
            {
                if(x>=dx && x<=dx+TextWidth(item.Text)+80 && y>=dy && y<=dy+Size.Y)
                {
                    OverItem = item;   
                }
                //DrawText(item.Text, dx, dy, new Vector4(0, 0, 0, 1));
                dx += TextWidth(item.Text) + 60;

            }
        }

        public MenuItem AddItem(string text, Texture.Texture2D icon = null)
        {

            var new_item = new MenuItem();

            new_item.Icon = icon;
            new_item.Text = text;

            Items.Add(new_item);

            return new_item;

        }

    }
}
